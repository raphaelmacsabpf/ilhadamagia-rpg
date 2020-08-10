﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Server.Application.Enums;
using Server.Application.Managers;
using System;
using System.Collections.Generic;

namespace Server.Application
{
    public class MainServer : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly PlayerActions playerActions;
        private readonly ChatManager chatManager;
        private readonly StateManager stateManager;

        public MainServer(PlayerInfo playerInfo, CommandManager commandManager, MapManager mapManager, NetworkManager networkManager, PlayerActions playerActions, ChatManager chatManager, StateManager stateManager)
        {
            this.playerInfo = playerInfo;
            this.CommandManager = commandManager;
            this.MapManager = mapManager;
            this.networkManager = networkManager;
            this.playerActions = playerActions;
            this.chatManager = chatManager;
            this.stateManager = stateManager;
            foreach (var player in this.Players)
            {
                stateManager.PrepareFSMForPlayer(player);
                var gfPlayer = playerInfo.GetGFPlayer(player);
                gfPlayer.FSM.Fire(PlayerConnectionTrigger.GAMEMODE_LOAD);
            }

            API.RegisterCommand("gmx", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source <= 0)
                {
                    foreach (var targetPlayer in playerInfo.GetGFPlayerList())
                    {
                        targetPlayer.Player.Drop("GMX");
                    }
                }
            }), true);

            Console.WriteLine("[IM MainServer] Started MainServer");
        }

        internal CommandManager CommandManager { get; }

        internal MapManager MapManager { get; }

        internal async void OnClientReady([FromSource] Player player)
        {
            stateManager.PrepareFSMForPlayer(player);
            var gfPlayer = playerInfo.GetGFPlayer(player);
            gfPlayer.FSM.Fire(PlayerConnectionTrigger.CLIENT_READY);
        }

        internal void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var gfPlayer = this.playerInfo.GetGFPlayer(player);
            gfPlayer.FSM.Fire(PlayerConnectionTrigger.PLAYER_DROPPED);
        }

        internal async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
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
            stateManager.SelectAccountForPlayer(gfPlayer, accountName);
        }

        public void OnPlayerTriggerStateEvent([FromSource] Player player, string eventTriggered)
        {
            var gfPlayer = playerInfo.GetGFPlayer(player);
            switch (eventTriggered)
            {
                case "die":
                {
                    if (gfPlayer.FSM.CanFire(PlayerConnectionTrigger.PLAYER_DIED))
                    {
                        gfPlayer.FSM.Fire(PlayerConnectionTrigger.PLAYER_DIED);
                    }
                    return;
                }
            }
        }
    }
}