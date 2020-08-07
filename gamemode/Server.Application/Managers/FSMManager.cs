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
    public class FSMManager
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly MapManager mapManager;
        private readonly PlayerActions playerActions;
        private readonly AccountRepository accountRepository;

        public FSMManager(ChatManager chatManager, PlayerInfo playerInfo, NetworkManager networkManager, MapManager mapManager, PlayerActions playerActions, AccountRepository accountRepository)
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

        private StateMachine<PlayerConnectionState, PlayerConnectionTrigger> CreatePlayerConnectionFSM(GFPlayer gfPlayer)
        {
            var fsm = new StateMachine<PlayerConnectionState, PlayerConnectionTrigger>(PlayerConnectionState.INITIAL);
            fsm.OnTransitioned((transition) =>
            {
                if (gfPlayer.Account != null)
                {
                    Console.WriteLine($"Player connection state change #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Username: {gfPlayer.Account.Username}, Transition: {transition.Source} -> {transition.Destination}");
                }
                else
                {
                    Console.WriteLine($"Player connection state change #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Transition: {transition.Source} -> {transition.Destination}");
                }
            });
            fsm.OnUnhandledTrigger((state, trigger) =>
            {
                if (gfPlayer.Account != null)
                {
                    Console.WriteLine($"ERROR: State trigger error  #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, Username: {gfPlayer.Account.Username}, State: {state}, Trigger: {trigger}");
                }
                else
                {
                    Console.WriteLine($"ERROR: State trigger error  #{gfPlayer.Player.Handle} Name: {gfPlayer.Player.Name}, State: {state}, Trigger: {trigger}");
                }
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
                    var accountsDto = gfPlayer.LicenseAccounts.Select((element) =>
                    {
                        return new
                        {
                            Username = element.Username,
                            Level = element.Level
                        };
                    });
                    var json = JsonConvert.SerializeObject(accountsDto);
                    var compressedJson = networkManager.Compress(json);
                    this.playerActions.OpenNUIView(gfPlayer, NUIViewType.SELECT_ACCOUNT, true, compressedJson, json.Length);
                });
            fsm.Configure(PlayerConnectionState.LOGGED)
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

                    playerActions.SpawnPlayer(gfPlayer, "S_M_Y_MARINE_01", 309.6f, -728.7297f, 29.3136f, 246.6142f); // HACK: Mandar player spawnar elegantemente
                });
            return fsm;
        }
    }
}