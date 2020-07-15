using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting;
using GF.CrossCutting.Dto;
using Newtonsoft.Json;
using Server.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Server
{
    public class MainServer : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly MapManager mapManager;
        private readonly NetworkManager networkManager;
        private readonly PlayerActions playerActions;
        private readonly ChatManager chatManager;
        private readonly Thread syncThread;
        private bool scriptStopped;

        public MainServer(PlayerInfo playerInfo, CommandManager commandManager, MapManager mapManager, NetworkManager networkManager, PlayerActions playerActions, ChatManager chatManager)
        {
            this.playerInfo = playerInfo;
            this.CommandManager = commandManager;
            this.mapManager = mapManager;
            this.networkManager = networkManager;
            this.playerActions = playerActions;
            this.chatManager = chatManager;

            foreach (var player in new PlayerList())
            {
                this.playerInfo.PlayerToGFPlayer(player);
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

        public void OnClientReady([FromSource]Player player)
        {
            var gfPlayer = playerInfo.PlayerToGFPlayer(player);
            var json = JsonConvert.SerializeObject(gfPlayer.PopUpdatedPlayerVarsPayload());
            this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_PLAYER_VARS, json);

            json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticMarkersPayload());
            this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_MARKERS, json);

            json = JsonConvert.SerializeObject(this.mapManager.PopUpdatedStaticTargetsPayload());
            this.networkManager.SendPayloadToPlayer(player, PayloadType.TO_STATIC_TARGETS, json);

            this.chatManager.SendClientMessage(player, ChatColor.TEAM_VAGOS_COLOR, "Chegou aqui que seu cliente ta suavão");
        }

        public async void OnPlayerConnecting([FromSource]Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            // Mandatory Wait by FiveM
            await Delay(0);

            Console.WriteLine($"[Connecting] {playerName}, IP: {player.EndPoint}, Identifiers: {player.Identifiers["license"]}");
            for(int i = 0; i < 100; i++)
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
