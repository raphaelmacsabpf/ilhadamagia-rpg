using CitizenFX.Core;
using Shared.CrossCutting;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Services;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Services;
using Stateless;
using Stateless.Graph;
using System;
using System.Linq;

namespace Server.Application.Managers
{
    public class StateManager
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly MapManager mapManager;
        private readonly AccountService accountService;
        private readonly OrgService orgService;
        private readonly PlayerService playerService;

        public StateManager(ChatManager chatManager, PlayerInfo playerInfo, MapManager mapManager, AccountService accountService, OrgService orgService, PlayerService playerService)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.mapManager = mapManager;
            this.accountService = accountService;
            this.orgService = orgService;
            this.playerService = playerService;
        }

        public void SelectAccountForPlayer(PlayerHandle playerHandle, string accountName)
        {
            var account = playerHandle.LicenseAccounts.FirstOrDefault((element) => element.Username == accountName);
            if (account == null) return;
            playerHandle.Account = account;
            if (account.SelectedHouse != null)
            {
                var houses = mapManager.GetAllHousesFromOwner(account.Username);
                playerHandle.SelectedHouse = houses.FirstOrDefault((gfHouse) => gfHouse != null && gfHouse.Id == account.SelectedHouse);
            }
            playerHandle.FSM.Fire(PlayerConnectionTrigger.ACCOUNT_SELECTED);
        }

        public StateMachine<PlayerConnectionState, PlayerConnectionTrigger> CreatePlayerConnectionFSM(PlayerHandle playerHandle)
        {
            var fsm = new StateMachine<PlayerConnectionState, PlayerConnectionTrigger>(PlayerConnectionState.INITIAL);

            fsm.OnTransitioned((transition) =>
            {
                Console.WriteLine(playerHandle.Account != null
                    ? $"Player connection state change #{playerHandle.Player.Handle} Name: {playerHandle.Player.Name}, Username: {playerHandle.Account.Username}, Transition: {transition.Source}[{transition.Trigger}] -> {transition.Destination}"
                    : $"Player connection state change #{playerHandle.Player.Handle} Name: {playerHandle.Player.Name}, Transition: {transition.Source}[{transition.Trigger}] -> {transition.Destination}");
            });

            fsm.OnUnhandledTrigger((state, trigger) =>
            {
                Console.WriteLine(playerHandle.Account != null
                    ? $"ERROR: State trigger error  #{playerHandle.Player.Handle} Name: {playerHandle.Player.Name}, Username: {playerHandle.Account.Username}, State: {state}, Trigger: {trigger}"
                    : $"ERROR: State trigger error  #{playerHandle.Player.Handle} Name: {playerHandle.Player.Name}, State: {state}, Trigger: {trigger}");
            });

            fsm.Configure(PlayerConnectionState.INITIAL)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .Permit(PlayerConnectionTrigger.CLIENT_READY, PlayerConnectionState.CONNECTED)
                .Permit(PlayerConnectionTrigger.GAMEMODE_LOAD, PlayerConnectionState.GAMEMODE_LOAD_DELAY);

            fsm.Configure(PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    if (playerHandle.Account != null)
                    {
                        var playerPosition = playerHandle.Player.Character.Position;
                        accountService.EndSession(playerHandle.Account, playerPosition.X, playerPosition.Y, playerPosition.Z, playerHandle.CurrentHouseInside);
                        Console.WriteLine($"Player dropped #{playerHandle.Player.Handle} Name: {playerHandle.Player.Name}, Username: {playerHandle.Account.Username}");
                        this.playerInfo.UnloadPlayerHandle(playerHandle);
                    }
                    else
                    {
                        Console.WriteLine($"Player dropped #{playerHandle.Player.Handle} Name: {playerHandle.Player.Name}");
                    }
                });

            fsm.Configure(PlayerConnectionState.GAMEMODE_LOAD_DELAY)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .Permit(PlayerConnectionTrigger.CLIENT_READY, PlayerConnectionState.CONNECTED);

            fsm.Configure(PlayerConnectionState.CONNECTED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .Permit(PlayerConnectionTrigger.ACCOUNT_FOUND, PlayerConnectionState.LOADING_ACCOUNT)
                .Permit(PlayerConnectionTrigger.ACCOUNT_NOT_FOUND, PlayerConnectionState.NEW_ACCOUNT)
                .OnEntry(async () =>
                {
                    playerHandle.SyncPlayerDateTime(mapManager.GetWorldClock(), mapManager.GetMillisecondsPerMinuteWorldClock());
                    var accounts = (await accountService.GetAccountListForLicense(playerHandle.License)).ToList();
                    if (accounts.Count > 0)
                    {
                        foreach (var account in accounts)
                        {
                            playerHandle.LicenseAccounts.Add(account);
                            Console.WriteLine($"Encontrada conta #{account.Id} para username: {account.Username}");
                        }
                        fsm.Fire(PlayerConnectionTrigger.ACCOUNT_FOUND);
                    }
                    else
                    {
                        fsm.Fire(PlayerConnectionTrigger.ACCOUNT_NOT_FOUND);
                    }
                });

            fsm.Configure(PlayerConnectionState.NEW_ACCOUNT)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(async () =>
                {
                    var json = JsonConvert.SerializeObject(null);
                    var compressedJson = LZ4Utils.Compress(json);
                    playerHandle.OpenNUIView(NUIViewType.CREATE_ACCOUNT, true, compressedJson, json.Length);
                });

            fsm.Configure(PlayerConnectionState.LOADING_ACCOUNT)
                .Permit(PlayerConnectionTrigger.ACCOUNT_SELECTED, PlayerConnectionState.LOGGED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    var accountsDto = playerHandle.LicenseAccounts.Select((element) => new
                    {
                        Username = element.Username,
                        Level = element.Level
                    });
                    var json = JsonConvert.SerializeObject(accountsDto);
                    var compressedJson = LZ4Utils.Compress(json);
                    playerHandle.OpenNUIView(NUIViewType.SELECT_ACCOUNT, true, compressedJson, json.Length);
                });

            fsm.Configure(PlayerConnectionState.LOGGED)
                .Permit(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION, PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    playerHandle.CloseNUIView(NUIViewType.SELECT_ACCOUNT, true);
                    var json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticMarkersPayload());
                    playerHandle.SendPayloadToPlayer(PayloadType.TO_STATIC_MARKERS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticProximityTargetsPayload());
                    playerHandle.SendPayloadToPlayer(PayloadType.TO_STATIC_PROXIMITY_TARGETS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticInteractionTargetsPayload());
                    playerHandle.SendPayloadToPlayer(PayloadType.TO_STATIC_INTERACTION_TARGETS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdateBlipsPayload());
                    playerHandle.SendPayloadToPlayer(PayloadType.TO_MAP_BLIPS, json);

                    playerHandle.FSM.Fire(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION);
                });

            fsm.Configure(PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.SWITCHED_OUT, PlayerConnectionState.SPAWNING)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry((transition) =>
                {
                    if (playerHandle.SpawnType == SpawnType.Unset)
                    {
                        if (DateTime.Now - playerHandle.Account.UpdatedAt < TimeSpan.FromSeconds(600))
                        {
                            var spawnPosition = new Vector3(playerHandle.Account.LastX, playerHandle.Account.LastY, playerHandle.Account.LastZ);
                            playerHandle.SpawnPosition = spawnPosition;
                            playerHandle.SwitchInPosition = spawnPosition;
                            var accountLastHouseInside = playerHandle.Account.LastHouseInside;
                            if (accountLastHouseInside != null)
                            {
                                var lastHouseInt = Convert.ToInt32(accountLastHouseInside);
                                playerHandle.CurrentHouseInside = mapManager.GetGFHouseFromId(lastHouseInt);
                            }
                            chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Última posição");
                        }
                        else if (playerHandle.SelectedHouse != null)
                        {
                            var houseEntity = playerHandle.SelectedHouse;
                            playerHandle.CurrentHouseInside = playerHandle.SelectedHouse;
                            playerHandle.SpawnPosition = mapManager.GetHouseInteriorPosition(playerHandle.SelectedHouse);
                            playerHandle.SwitchInPosition = new Vector3(houseEntity.EntranceX, houseEntity.EntranceY, houseEntity.EntranceZ);
                            chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Interior da casa");
                        }
                        else
                        {
                            SetSpawnToOrganization(playerHandle);
                            chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Spawn organização");
                        }
                    }
                    playerHandle.SwitchOutPlayer();
                });

            fsm.Configure(PlayerConnectionState.SPAWNING)
                .PermitReentry(PlayerConnectionTrigger.SET_TO_SPAWN)
                .Permit(PlayerConnectionTrigger.SWITCHED_IN, PlayerConnectionState.SPAWNED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    this.playerService.SpawnPlayer(playerHandle);
                });

            fsm.Configure(PlayerConnectionState.SPAWNED)
                .Permit(PlayerConnectionTrigger.SET_TO_SPAWN, PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.PLAYER_DIED, PlayerConnectionState.DIED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    playerHandle.SyncPlayerDateTime(this.mapManager.GetWorldClock(), this.mapManager.GetMillisecondsPerMinuteWorldClock());
                    this.playerInfo.SendUpdatedPlayerVars(playerHandle);
                });

            fsm.Configure(PlayerConnectionState.DIED)
                .Permit(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION, PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    // TODO: Tratar morte do jogador
                    fsm.Fire(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION);
                });

            // TODO: Versionar arquivo de gráfico de estados
            var graphText = UmlDotGraph.Format(fsm.GetInfo());
            Console.WriteLine(graphText);

            return fsm;
        }

        private void SetSpawnToOrganization(PlayerHandle playerHandle)
        {
            Org playerOrg = orgService.GetAccountOrg(playerHandle.Account);
            Vector3 spawnPosition;
            if (playerOrg == null)
            {
                spawnPosition = new Vector3(309.6f, -728.7297f, 29.3136f);
            }
            else
            {
                spawnPosition = new Vector3(playerOrg.SpawnX, playerOrg.SpawnY, playerOrg.SpawnZ);
            }

            playerHandle.SpawnPosition = spawnPosition;
            playerHandle.SwitchInPosition = spawnPosition;
            playerHandle.SpawnDimension = 1;
        }
    }
}