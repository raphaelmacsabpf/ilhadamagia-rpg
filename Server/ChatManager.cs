using CitizenFX.Core;
using GF.CrossCutting;
using System;

namespace Server
{
    public class ChatManager : BaseScript
    {
        public ChatManager(bool ignoreFiveMInitialization) // TODO: Improve useless parameter Hack
        {
            Console.WriteLine("[IM ChatManager] Started ChatManager");
        }

        public void SendClientMessageToAll(ChatColor chatColor, string message)
        {
            foreach (Player player in new PlayerList())
            {
                SendClientMessage(player, chatColor, message);
            }
        }

        public void SendClientMessage(Player player, ChatColor chatColor, string message)
        {
            player.TriggerEvent("GF:Client:SendClientMessage", (int)chatColor, message);
        }

        public void PlayerChat(Player player, string message)
        {
            ProxDetectorChat(20.0f, player, message);
        }

        public void PlayerScream(Player player, string message)
        {
            var outputMessage = $"[ID: {player.Handle}] {player.Name} GRITA: {message.ToUpper()}!!!";
            ProxDetectorChat(30.0f, player, outputMessage);
        }

        public void ProxDetectorColors(float radi, Player player, string message, ChatColor color1, ChatColor color2, ChatColor color3, ChatColor color4, ChatColor color5)
        {
            foreach (Player i in new PlayerList())
            {
                var position = i.Character.Position;
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

        internal void OnChatMessage([FromSource] Player player, string message)
        {
            var wholeMessageCharsIsUppercase = (message.CompareTo(message.ToUpper()) == 0);
            if (wholeMessageCharsIsUppercase)
            {
                PlayerScream(player, message);
            }
            else
            {
                var messageToChat = $"[ID: {player.Handle}] {player.Name} diz: {message}";
                PlayerChat(player, messageToChat);
            }
        }

        private void ProxDetectorChat(float radi, Player player, string message)
        {
            ProxDetectorColors(radi, player, message, ChatColor.COLOR_FADE1, ChatColor.COLOR_FADE2, ChatColor.COLOR_FADE3, ChatColor.COLOR_FADE4, ChatColor.COLOR_FADE5);
        }
    }
}