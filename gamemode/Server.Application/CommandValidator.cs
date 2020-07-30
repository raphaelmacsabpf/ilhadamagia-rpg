using Server.Application.Entities;
using Server.Application.Managers;
using Shared.CrossCutting;
using System;
using System.Collections.Generic;

namespace Server.Application
{
    public class CommandValidator
    {
        private readonly PlayerInfo playerInfo;
        private readonly ChatManager chatManager;
        private GFPlayer sourceGFPlayer;
        private GFPlayer targetGFPlayer;
        private readonly string[] commandArgs;
        private int nextArgPosition;
        private List<string> errorMessages;

        public CommandValidator(PlayerInfo playerInfo, ChatManager chatManager, GFPlayer gfPlayer, string[] commandArgs)
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.sourceGFPlayer = gfPlayer;
            this.commandArgs = commandArgs;
            this.nextArgPosition = 1; // Default first argument to validate
            this.errorMessages = new List<string>();
        }

        public GFPlayer GetTargetGFPlayer()
        {
            if (this.targetGFPlayer == null)
            {
                throw new InvalidOperationException("GetTargetGFPlayer() called without TargetPlayer validation");
            }
            return this.targetGFPlayer;
        }

        public CommandValidator AdminLevel(int minAdminLevel)
        {
            if (this.sourceGFPlayer.Account.AdminLevel < minAdminLevel)
            {
                this.errorMessages.Add("Você não está autorizado a utilizar este comando");
            }
            return this;
        }

        public CommandValidator TargetPlayer()
        {
            string nextArgumentValue = GetNextArgumentValue();
            if (nextArgumentValue != null)
            {
                this.targetGFPlayer = GetPlayerByIdOrName(nextArgumentValue);
                if (this.targetGFPlayer == null)
                {
                    this.errorMessages.Add($"{nextArgumentValue} não é um id/username válido");
                }
            }
            else
            {
                this.errorMessages.Add("Informe um id/username válido");
            }

            return this;
        }

        public bool IsValid()
        {
            foreach (var errorMessage in this.errorMessages)
            {
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD2, errorMessage);
            }
            this.nextArgPosition = 1;
            this.targetGFPlayer = null;
            return this.errorMessages.Count == 0;
        }

        private string GetNextArgumentValue()
        {
            if (commandArgs.Length > nextArgPosition)
            {
                var nextArgumentValue = commandArgs[nextArgPosition];
                nextArgPosition++;
                return nextArgumentValue;
            }
            return null;
        }

        private GFPlayer GetPlayerByIdOrName(string playerStr)
        {
            int parsedId;
            bool parseSucceed = Int32.TryParse(playerStr, out parsedId);
            var gfPlayerList = this.playerInfo.GetGFPlayerList();
            if (parseSucceed)
            {
                foreach (var gfPlayer in gfPlayerList)
                {
                    if (Int32.Parse(gfPlayer.Player.Handle) == parsedId)
                    {
                        return gfPlayer;
                    }
                }
            }
            else
            {
                var loweredPlayerStr = playerStr.ToLower();
                foreach (var gfPlayer in gfPlayerList)
                {
                    var loweredUsername = gfPlayer.Account.Username.ToLower();
                    if (loweredUsername.StartsWith(loweredPlayerStr) || loweredUsername == loweredPlayerStr)
                    {
                        return gfPlayer;
                    }
                }
            }

            return null;
        }
    }
}