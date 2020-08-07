using CitizenFX.Core;
using Server.Application.Entities;
using Shared.CrossCutting;
using System;

namespace Server.Application.Managers
{
    public class ChatManager : BaseScript
    {
        private readonly PlayerInfo playerInfo;

        public ChatManager(PlayerInfo playerInfo)
        {
            Console.WriteLine("[IM ChatManager] Started ChatManager");
            this.playerInfo = playerInfo;
        }

        public void SendClientMessageToAll(ChatColor chatColor, string message)
        {
            foreach (GFPlayer gfPlayer in playerInfo.GetGFPlayerList())
            {
                SendClientMessage(gfPlayer, chatColor, message);
            }
        }

        public void SendClientMessage(GFPlayer gfPlayer, ChatColor chatColor, string message)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SendClientMessage", (int)chatColor, message);
        }

        public void PlayerChat(GFPlayer gfPlayer, string message)
        {
            ProxDetectorChat(20.0f, gfPlayer, message);
        }

        public void PlayerScream(GFPlayer gfPlayer, string message)
        {
            var outputMessage = $"[ID: {gfPlayer.Player.Handle}] {gfPlayer.Account.Username} GRITA: {message.ToUpper()}!!!";
            ProxDetectorChat(30.0f, gfPlayer, outputMessage);
        }

        public void ProxDetectorColors(float radi, GFPlayer gfPlayer, string message, ChatColor color1, ChatColor color2, ChatColor color3, ChatColor color4, ChatColor color5)
        {
            var player = gfPlayer.Player;
            foreach (GFPlayer i in this.playerInfo.GetGFPlayerList())
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

        internal void OnChatMessage([FromSource] Player player, string message) // TODO: PROTEGER OnChatMessage
        {
            var wholeMessageCharsIsUppercase = (message.CompareTo(message.ToUpper()) == 0);
            var gfPlayer = playerInfo.GetGFPlayer(player);
            if (wholeMessageCharsIsUppercase)
            {
                PlayerScream(gfPlayer, message);
            }
            else
            {
                var messageToChat = $"[ID: {player.Handle}] {gfPlayer.Account.Username} diz: {message}";
                PlayerChat(gfPlayer, messageToChat);
            }
        }

        private void ProxDetectorChat(float radi, GFPlayer gfPlayer, string message)
        {
            ProxDetectorColors(radi, gfPlayer, message, ChatColor.COLOR_FADE1, ChatColor.COLOR_FADE2, ChatColor.COLOR_FADE3, ChatColor.COLOR_FADE4, ChatColor.COLOR_FADE5);
        }
    }
}