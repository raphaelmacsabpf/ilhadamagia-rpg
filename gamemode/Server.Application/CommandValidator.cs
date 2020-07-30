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
        private Dictionary<string, int> commandVariablesInt;

        public CommandValidator(PlayerInfo playerInfo, ChatManager chatManager, GFPlayer gfPlayer, string[] commandArgs)
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.sourceGFPlayer = gfPlayer;
            this.commandArgs = commandArgs;
            this.nextArgPosition = 1; // Default first argument to validate
            this.errorMessages = new List<string>();
            this.commandVariablesInt = new Dictionary<string, int>();
        }

        public CommandValidator WithAdminLevel(int minAdminLevel)
        {
            if (this.sourceGFPlayer.Account.AdminLevel < minAdminLevel)
            {
                this.errorMessages.Add("Você não está autorizado a utilizar este comando");
            }
            return this;
        }

        public CommandValidator WithTargetPlayer()
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

        public CommandValidator WithValueBetween(int min, int max, string variableName)
        {
            string nextArgumentValue = GetNextArgumentValue();
            if (nextArgumentValue != null)
            {
                int nextArgumentValueInt;
                bool parseStatus = Int32.TryParse(nextArgumentValue, out nextArgumentValueInt);
                if (parseStatus == false || nextArgumentValueInt < min || nextArgumentValueInt > max)
                {
                    this.errorMessages.Add($"{nextArgumentValue} não é um valor válido por não estar entre {min} e {max}");
                }
                else if(parseStatus == true)
                {
                    commandVariablesInt.Add(variableName, nextArgumentValueInt);
                } 
            }
            else
            {
                this.errorMessages.Add("Informe um valor válido");
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
            return this.errorMessages.Count == 0;
        }

        public GFPlayer GetTargetGFPlayer()
        {
            if (this.targetGFPlayer == null)
            {
                throw new InvalidOperationException("GetTargetGFPlayer() called without TargetPlayer validation");
            }
            return this.targetGFPlayer;
        }

        public int GetVariableInt(string variableName)
        {
            int variableValue;
            if(commandVariablesInt.TryGetValue(variableName, out variableValue) == false) {
                throw new InvalidOperationException($"GetVariableInt() failed to locate variable value for: {variableName}");
            }
            return variableValue;
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