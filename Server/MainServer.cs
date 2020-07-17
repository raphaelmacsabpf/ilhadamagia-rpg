using CitizenFX.Core;
using GF.CrossCutting;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace Server
{
    public class MainServer : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly PlayerActions playerActions;
        private readonly ChatManager chatManager;
        private readonly Thread syncThread;
        private bool scriptStopped;
        private int activePlayers;

        public MainServer(PlayerInfo playerInfo, CommandManager commandManager, MapManager mapManager, NetworkManager networkManager, PlayerActions playerActions, ChatManager chatManager)
        {
            this.playerInfo = playerInfo;
            this.CommandManager = commandManager;
            this.MapManager = mapManager;
            this.networkManager = networkManager;
            this.playerActions = playerActions;
            this.chatManager = chatManager;

            foreach (var player in new PlayerList())
            {
                var gfPlayer = this.playerInfo.PlayerToGFPlayer(player);
                gfPlayer.IsActive = true;
                activePlayers++;
                Console.WriteLine($"[IM MainServer] PlayerLoaded: [{player.Handle}] {player.Name}");
            }

            /*this.syncThread = new Thread(SyncThreadHandler);
            this.syncThread.Priority = ThreadPriority.Highest;
            this.syncThread.IsBackground = true;
            this.syncThread.Start();*/

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
            var gfPlayer = playerInfo.PlayerToGFPlayer(player);
            var json = JsonConvert.SerializeObject(gfPlayer.PopUpdatedPlayerVarsPayload());
            this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_PLAYER_VARS, json);

            json = JsonConvert.SerializeObject(this.MapManager.PopUpdatedStaticMarkersPayload());
            this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_MARKERS, json);

            json = JsonConvert.SerializeObject(this.MapManager.PopUpdatedStaticProximityTargetsPayload());
            this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_PROXIMITY_TARGETS, json);

            json = JsonConvert.SerializeObject(this.MapManager.PopUpdatedStaticInteractionTargetsPayload());
            this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_INTERACTION_TARGETS, json);

            this.chatManager.SendClientMessage(player, ChatColor.TEAM_VAGOS_COLOR, "Chegou aqui que seu cliente ta suavão");
            this.activePlayers++;
            gfPlayer.IsActive = true;
        }

        internal void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var gfPlayer = this.playerInfo.PlayerToGFPlayer(player);
            gfPlayer.IsActive = false;
            activePlayers--;
            Console.WriteLine($"Player dropped, name: {gfPlayer.Username}, reason: {reason}, activePlayers: {activePlayers}");
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

            this.playerInfo.PlayerToGFPlayer(player);

            Console.WriteLine($"[Connected] {playerName}, IP: {player.EndPoint}, Identifiers: {player.Identifiers["license"]}");
            deferrals.done();
        }
    }
}