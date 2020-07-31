using CitizenFX.Core;
using GF.CrossCutting;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Managers;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using System;
using System.Linq;

namespace Server.Application
{
    public class MainServer : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly PlayerActions playerActions;
        private readonly ChatManager chatManager;

        public MainServer(PlayerInfo playerInfo, CommandManager commandManager, MapManager mapManager, NetworkManager networkManager, PlayerActions playerActions, ChatManager chatManager)
        {
            this.playerInfo = playerInfo;
            this.CommandManager = commandManager;
            this.MapManager = mapManager;
            this.networkManager = networkManager;
            this.playerActions = playerActions;
            this.chatManager = chatManager;
            foreach (var player in this.Players)
            {
                var gfPlayer = this.playerInfo.GetGFPlayer(player);
                gfPlayer.ConnectionState = PlayerConnectionState.ON_GAMEMODE_LOAD;
                //Console.WriteLine($"[IM MainServer] PlayerLoaded: [{player.Handle}] {gfPlayer.Account.Username}"); // TODO: Resolver este log
            }

            Console.WriteLine("[IM MainServer] Started MainServer");
        }

        /*~MainServer()
        {
            Console.WriteLine("Destruction!!!!");
            Thread.Sleep(3000);
        }*/
        public CommandManager CommandManager { get; }

        public MapManager MapManager { get; }

        public async void OnClientReady([FromSource] Player player)
        {
            var gfPlayer = playerInfo.GetGFPlayer(player);
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
        }

        internal void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var gfPlayer = this.playerInfo.GetGFPlayer(player);
            this.playerInfo.UnloadGFPlayer(gfPlayer); // fTODO: Melhorar remoção do GFPlayer
            gfPlayer.ConnectionState = PlayerConnectionState.DROPPED;
            Console.WriteLine($"Player dropped, name: {gfPlayer.Account.Username}, reason: {reason}");
        }

        public async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            // Mandatory Wait by FiveM
            await Delay(0);

            Console.WriteLine($"[Connecting] {playerName}, IP: {player.EndPoint}, Identifiers: {player.Identifiers["license"]}");
            for (int i = 0; i < 100; i++)
            {
                deferrals.update("Still checking:" + (i + 1));

                await Delay(20);
            }
            Console.WriteLine($"[Connected] ID:{player.Handle}, PlayerName: {playerName}, IP: {player.EndPoint}, License: {player.Identifiers["license"]}");
            deferrals.done();
        }

        internal void OnPlayerSelectAccount([FromSource] Player player, string accountName)
        {
            var gfPlayer = playerInfo.GetGFPlayer(player);
            var account = gfPlayer.LicenseAccounts.FirstOrDefault((element) =>
            {
                return element.Username == accountName;
            });

            if (account != null)
            {
                Console.WriteLine("User selected valid account: " + account.Username); // TODO: Remover este log
                gfPlayer.Account = account;
                playerActions.CloseNUIView(gfPlayer, NUIViewType.SELECT_ACCOUNT, true);

                var json = JsonConvert.SerializeObject(PopUpdatedPlayerVarsPayload(gfPlayer));
                this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_PLAYER_VARS, json);
                json = JsonConvert.SerializeObject(this.MapManager.PopUpdatedStaticMarkersPayload());
                this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_MARKERS, json);
                json = JsonConvert.SerializeObject(this.MapManager.PopUpdatedStaticProximityTargetsPayload());
                this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_PROXIMITY_TARGETS, json);
                json = JsonConvert.SerializeObject(this.MapManager.PopUpdatedStaticInteractionTargetsPayload());
                this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_INTERACTION_TARGETS, json);

                playerActions.SpawnPlayer(gfPlayer, "S_M_Y_MARINE_01", 309.6f, -728.7297f, 29.3136f, 246.6142f); // HACK: Mandar player spawnar elegantemente
            }
            else
            {
                Console.WriteLine("[ERROR] USER SELECTED INVALID ACCOUNT: " + accountName); // TODO: Remover este log e o else
            }
        }

        private PlayerVarsDto PopUpdatedPlayerVarsPayload(GFPlayer gfPlayer)
        {
            PlayerVarsDto playerVars = new PlayerVarsDto();
            playerVars.TryAdd("Money", gfPlayer.Account.Money.ToString());
            playerVars.TryAdd("Username", gfPlayer.Account.Username);
            return playerVars;
        }
    }
}