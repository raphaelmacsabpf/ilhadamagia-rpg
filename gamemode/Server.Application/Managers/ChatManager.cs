using CitizenFX.Core;
using GF.CrossCutting.Enums;
using Server.Application.Entities;
using Shared.CrossCutting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Application.Managers
{
    public class ChatManager
    {
        private readonly PlayerInfo playerInfo;

        public ChatManager(PlayerInfo playerInfo)
        {
            Console.WriteLine("[IM ChatManager] Started ChatManager");
            this.playerInfo = playerInfo;
        }

        public void SendClientMessageToAll(ChatColor chatColor, string message)
        {
            foreach (PlayerHandle playerHandle in playerInfo.GetPlayerHandleList())
            {
                SendClientMessage(playerHandle, chatColor, message);
            }
        }

        public void SendClientMessage(PlayerHandle playerHandle, ChatColor chatColor, string message)
        {
            playerHandle.CallClientAction(ClientEvent.SendClientMessage, (int)chatColor, message);
        }

        public void PlayerScream(PlayerHandle playerHandle, string message)
        {
            var outputMessage = $"[ID: {playerHandle.Player.Handle}] {playerHandle.Account.Username} GRITA: {message.ToUpper()}!!!";
            ProxDetectorChat(30.0f, playerHandle, outputMessage);
        }

        public void ProxDetectorColors(float radi, PlayerHandle playerHandle, string message, ChatColor color1, ChatColor color2, ChatColor color3, ChatColor color4, ChatColor color5)
        {
            var player = playerHandle.Player;
            foreach (PlayerHandle i in this.playerInfo.GetPlayerHandleList())
            {
                var position = i.Player.Character.Position;
                var tempposx = player.Character.Position.X - position.X;
                var tempposy = player.Character.Position.Y - position.Y;
                var tempposz = player.Character.Position.Z - position.Z;

                if (((tempposx < radi / 16.0f) && (tempposx > -radi / 16)) && ((tempposy < radi / 16) && (tempposy > -radi / 16)) && ((tempposz < radi / 16) && (tempposz > -radi / 16)))
                {
                    SendClientMessage(i, color1, message);
                }
                else if (((tempposx < radi / 8) && (tempposx > -radi / 8)) && ((tempposy < radi / 8) && (tempposy > -radi / 8)) && ((tempposz < radi / 8) && (tempposz > -radi / 8)))
                {
                    SendClientMessage(i, color2, message);
                }
                else if (((tempposx < radi / 4) && (tempposx > -radi / 4)) && ((tempposy < radi / 4) && (tempposy > -radi / 4)) && ((tempposz < radi / 4) && (tempposz > -radi / 4)))
                {
                    SendClientMessage(i, color3, message);
                }
                else if (((tempposx < radi / 2) && (tempposx > -radi / 2)) && ((tempposy < radi / 2) && (tempposy > -radi / 2)) && ((tempposz < radi / 2) && (tempposz > -radi / 2)))
                {
                    SendClientMessage(i, color4, message);
                }
                else if (((tempposx < radi) && (tempposx > -radi)) && ((tempposy < radi) && (tempposy > -radi)) && ((tempposz < radi) && (tempposz > -radi)))
                {
                    SendClientMessage(i, color5, message);
                }
            }
        }

        public void ProxDetectorColorFixed(float radi, PlayerHandle playerHandle, string message, ChatColor color, IEnumerable<PlayerHandle> ignoredPlayers)
        {
            var player = playerHandle.Player;
            var playerList = this.playerInfo.GetPlayerHandleList().Except(ignoredPlayers);
            foreach (PlayerHandle i in playerList)
            {
                var position = i.Player.Character.Position;
                var tempposx = player.Character.Position.X - position.X;
                var tempposy = player.Character.Position.Y - position.Y;
                var tempposz = player.Character.Position.Z - position.Z;

                if (((tempposx < radi) && (tempposx > -radi)) && ((tempposy < radi) && (tempposy > -radi)) && ((tempposz < radi) && (tempposz > -radi)))
                {
                    SendClientMessage(i, color, message);
                }
            }
        }

        private void ProxDetectorChat(float radi, PlayerHandle playerHandle, string message)
        {
            ProxDetectorColors(radi, playerHandle, message, ChatColor.COLOR_FADE1, ChatColor.COLOR_FADE2, ChatColor.COLOR_FADE3, ChatColor.COLOR_FADE4, ChatColor.COLOR_FADE5);
        }

        internal void PlayerChat(PlayerHandle playerHandle, string message)
        {
            ProxDetectorChat(20.0f, playerHandle, message);
        }
    }
}