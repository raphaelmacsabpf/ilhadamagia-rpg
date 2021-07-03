using CitizenFX.Core;
using GF.CrossCutting;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Services;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Services;
using Shared.CrossCutting;
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
        private readonly NetworkManager networkManager;
        private readonly MapManager mapManager;
        private readonly PlayerActions playerActions;
        private readonly AccountService accountService;
        private readonly OrgService orgService;
        private readonly PlayerService playerService;

        public StateManager(ChatManager chatManager, PlayerInfo playerInfo, NetworkManager networkManager, MapManager mapManager, PlayerActions playerActions, AccountService accountService, OrgService orgService, PlayerService playerService)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.networkManager = networkManager;
            this.mapManager = mapManager;
            this.playerActions = playerActions;
            this.accountService = accountService;
            this.orgService = orgService;
            this.playerService = playerService;
        }

        public void SelectAccountForPlayer(GFPlayer gfPlayer, string accountName)
        {
            var account = gfPlayer.LicenseAccounts.FirstOrDefault((element) => element.Username == accountName);
            if (account == null) return;
            gfPlayer.Account = account;
            if (account.SelectedHouse != null)
            {
                var houses = mapManager.GetAllHousesFromOwner(account.Username);
                gfPlayer.SelectedHouse = houses.FirstOrDefault((gfHouse) => gfHouse != null && gfHouse.Id == account.SelectedHouse);
            }
            gfPlayer.FSM.Fire(PlayerConnectionTrigger.ACCOUNT_SELECTED);
        }

        public StateMachine<PlayerConnectionState, PlayerConnectionTrigger> CreatePlayerConnectionFSM(GFPlayer gfPlayer)
        {
            var fsm = new StateMachine<PlayerConnectionState, PlayerConnectionTrigger>(PlayerConnectionState.INITIAL);

            fsm.OnTransitioned((transition) =>
            {
                Console.WriteLine(gfPlayer.Account != null
                    ? $"Player connection state change #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Username: {gfPlayer.Account.Username}, Transition: {transition.Source}[{transition.Trigger}] -> {transition.Destination}"
                    : $"Player connection state change #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Transition: {transition.Source}[{transition.Trigger}] -> {transition.Destination}");
            });

            fsm.OnUnhandledTrigger((state, trigger) =>
            {
                Console.WriteLine(gfPlayer.Account != null
                    ? $"ERROR: State trigger error  #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Username: {gfPlayer.Account.Username}, State: {state}, Trigger: {trigger}"
                    : $"ERROR: State trigger error  #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, State: {state}, Trigger: {trigger}");
            });

            fsm.Configure(PlayerConnectionState.INITIAL)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .Permit(PlayerConnectionTrigger.CLIENT_READY, PlayerConnectionState.CONNECTED)
                .Permit(PlayerConnectionTrigger.GAMEMODE_LOAD, PlayerConnectionState.GAMEMODE_LOAD_DELAY);

            fsm.Configure(PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    if (gfPlayer.Account != null)
                    {
                        var playerPosition = gfPlayer.Player.Character.Position;
                        accountService.EndSession(gfPlayer.Account, playerPosition.X, playerPosition.Y, playerPosition.Z, gfPlayer.CurrentHouseInside);
                        Console.WriteLine($"Player dropped #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Username: {gfPlayer.Account.Username}");
                        this.playerInfo.UnloadGFPlayer(gfPlayer);
                    }
                    else
                    {
                        Console.WriteLine($"Player dropped #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}");
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
                    this.networkManager.SyncPlayerDateTime(gfPlayer);
                    var accounts = (await accountService.GetAccountListForLicense(gfPlayer.License)).ToList();
                    if (accounts.Count > 0)
                    {
                        foreach (var account in accounts)
                        {
                            gfPlayer.LicenseAccounts.Add(account);
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
                    var compressedJson = networkManager.Compress(json);
                    this.playerActions.OpenNUIView(gfPlayer, NUIViewType.CREATE_ACCOUNT, true, compressedJson, json.Length);
                });

            fsm.Configure(PlayerConnectionState.LOADING_ACCOUNT)
                .Permit(PlayerConnectionTrigger.ACCOUNT_SELECTED, PlayerConnectionState.LOGGED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    var accountsDto = gfPlayer.LicenseAccounts.Select((element) => new
                    {
                        Username = element.Username,
                        Level = element.Level
                    });
                    var json = JsonConvert.SerializeObject(accountsDto);
                    var compressedJson = networkManager.Compress(json);
                    this.playerActions.OpenNUIView(gfPlayer, NUIViewType.SELECT_ACCOUNT, true, compressedJson, json.Length);
                });

            fsm.Configure(PlayerConnectionState.LOGGED)
                .Permit(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION, PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    this.playerActions.CloseNUIView(gfPlayer, NUIViewType.SELECT_ACCOUNT, true);
                    var json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticMarkersPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_STATIC_MARKERS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticProximityTargetsPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_STATIC_PROXIMITY_TARGETS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticInteractionTargetsPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_STATIC_INTERACTION_TARGETS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdateBlipsPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_MAP_BLIPS, json);

                    gfPlayer.FSM.Fire(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION);
                });

            fsm.Configure(PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.SWITCHED_OUT, PlayerConnectionState.SPAWNING)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry((transition) =>
                {
                    if (gfPlayer.SpawnType == SpawnType.Unset)
                    {
                        if (gfPlayer.IsFirstSpawn)
                        {
                            if (DateTime.Now - gfPlayer.Account.UpdatedAt < TimeSpan.FromSeconds(600))
                            {
                                var spawnPosition = new Vector3(gfPlayer.Account.LastX, gfPlayer.Account.LastY, gfPlayer.Account.LastZ);
                                gfPlayer.SpawnPosition = spawnPosition;
                                gfPlayer.SwitchInPosition = spawnPosition;
                                var accountLastHouseInside = gfPlayer.Account.LastHouseInside;
                                if (accountLastHouseInside != null)
                                {
                                    var lastHouseInt = Convert.ToInt32(accountLastHouseInside);
                                    gfPlayer.CurrentHouseInside = mapManager.GetGFHouseFromId(lastHouseInt);
                                }
                                chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Última posição");
                            }
                            else if (gfPlayer.SelectedHouse != null)
                            {
                                var houseEntity = gfPlayer.SelectedHouse;
                                gfPlayer.CurrentHouseInside = gfPlayer.SelectedHouse;
                                gfPlayer.SpawnPosition = mapManager.GetHouseInteriorPosition(gfPlayer.SelectedHouse);
                                gfPlayer.SwitchInPosition = new Vector3(houseEntity.EntranceX, houseEntity.EntranceY, houseEntity.EntranceZ);
                                chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Interior da casa");
                            }
                            else
                            {
                                SetSpawnToOrganization(gfPlayer);
                                chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Spawn organização");
                            }
                            gfPlayer.IsFirstSpawn = false;
                        }
                        else
                        {
                            SetSpawnToOrganization(gfPlayer);
                            chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_LIGHTBLUE, "Local de nascimento: Spawn organização");
                        }
                    }
                    this.playerActions.SwitchOutPlayer(gfPlayer);
                });

            fsm.Configure(PlayerConnectionState.SPAWNING)
                .PermitReentry(PlayerConnectionTrigger.SET_TO_SPAWN)
                .Permit(PlayerConnectionTrigger.SWITCHED_IN, PlayerConnectionState.SPAWNED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    this.playerService.SpawnPlayer(gfPlayer);
                });

            fsm.Configure(PlayerConnectionState.SPAWNED)
                .Permit(PlayerConnectionTrigger.SET_TO_SPAWN, PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.PLAYER_DIED, PlayerConnectionState.DIED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    this.networkManager.SyncPlayerDateTime(gfPlayer);
                    this.playerInfo.SendUpdatedPlayerVars(gfPlayer);
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

        private void SetSpawnToOrganization(GFPlayer gfPlayer)
        {
            Org playerOrg = orgService.GetAccountOrg(gfPlayer.Account);
            Vector3 spawnPosition;
            if (playerOrg == null)
            {
                spawnPosition = new Vector3(309.6f, -728.7297f, 29.3136f);
            }
            else
            {
                spawnPosition = new Vector3(playerOrg.SpawnX, playerOrg.SpawnY, playerOrg.SpawnZ);
            }

            gfPlayer.SpawnPosition = spawnPosition;
            gfPlayer.SwitchInPosition = spawnPosition;
        }
    }
}