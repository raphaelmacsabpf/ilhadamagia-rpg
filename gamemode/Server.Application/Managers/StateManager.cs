using CitizenFX.Core;
using GF.CrossCutting;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Database;
using Shared.CrossCutting;
using Stateless;
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
        private readonly AccountRepository accountRepository;

        public StateManager(ChatManager chatManager, PlayerInfo playerInfo, NetworkManager networkManager, MapManager mapManager, PlayerActions playerActions, AccountRepository accountRepository)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.networkManager = networkManager;
            this.mapManager = mapManager;
            this.playerActions = playerActions;
            this.accountRepository = accountRepository;
        }

        public void PrepareFSMForPlayer(Player player)
        {
            var gfPlayer = new GFPlayer(player);
            var fsm = CreatePlayerConnectionFSM(gfPlayer);
            gfPlayer.FSM = fsm;
            playerInfo.LoadGFPlayer(gfPlayer);
        }

        public void SelectAccountForPlayer(GFPlayer gfPlayer, string accountName)
        {
            var account = gfPlayer.LicenseAccounts.FirstOrDefault((element) => element.Username == accountName);
            if (account == null) return;
            gfPlayer.Account = account;
            if (account.SelectedHouse != null)
            {
                var houses = mapManager.GetAllHousesFromOwner(account.Username).ToList();
                gfPlayer.SelectedHouse = houses.FirstOrDefault((gfHouse) => gfHouse.Entity != null && gfHouse.Entity.Id == account.SelectedHouse );
            }
            gfPlayer.FSM.Fire(PlayerConnectionTrigger.ACCOUNT_SELECTED);
        }

        private StateMachine<PlayerConnectionState, PlayerConnectionTrigger> CreatePlayerConnectionFSM(GFPlayer gfPlayer)
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
                        accountRepository.Update(gfPlayer.Account);
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
                    var accounts = (await accountRepository.GetAccountListByLicense(gfPlayer.License)).ToList();
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

                    var json = JsonConvert.SerializeObject(this.playerInfo.PopUpdatedPlayerVarsPayload(gfPlayer));
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_PLAYER_VARS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticMarkersPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_STATIC_MARKERS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticProximityTargetsPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_STATIC_PROXIMITY_TARGETS, json);
                    json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticInteractionTargetsPayload());
                    this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_STATIC_INTERACTION_TARGETS, json);

                    gfPlayer.FSM.Fire(PlayerConnectionTrigger.SELECTING_SPAWN_POSITION);
                });

            fsm.Configure(PlayerConnectionState.SELECT_SPAWN_POSITION)
                .Permit(PlayerConnectionTrigger.SET_TO_SPAWN, PlayerConnectionState.SPAWNED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    if(gfPlayer.SelectedHouse != null)
                    {
                        gfPlayer.CurrentHouseInside = gfPlayer.SelectedHouse;
                        gfPlayer.SpawnPosition = mapManager.GetHouseInteriorPosition(gfPlayer.SelectedHouse);
                    }
                    else
                    {
                        gfPlayer.SpawnPosition = new Vector3(309.6f, -728.7297f, 29.3136f);
                    }
                    fsm.Fire(PlayerConnectionTrigger.SET_TO_SPAWN);
                });

            fsm.Configure(PlayerConnectionState.SPAWNED)
                .Permit(PlayerConnectionTrigger.PLAYER_DROPPED, PlayerConnectionState.DROPPED)
                .OnEntry(() =>
                {
                    playerActions.SpawnPlayer(gfPlayer, "S_M_Y_MARINE_01", gfPlayer.SpawnPosition.X, gfPlayer.SpawnPosition.Y, gfPlayer.SpawnPosition.Z, 0); // HACK: Pegar skin do banco para spawnar
                });
            return fsm;
        }
    }
}