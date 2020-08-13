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
        private readonly CommandPacket commandPacket;
        private GFPlayer targetGFPlayer;
        private readonly string[] commandArgs;
        private int nextArgPosition;
        private List<string> errorMessages;
        private Dictionary<string, object> commandVariables;

        public CommandValidator(PlayerInfo playerInfo, ChatManager chatManager, GFPlayer gfPlayer, CommandPacket commandPacket)
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.sourceGFPlayer = gfPlayer;
            this.commandPacket = commandPacket;
            this.commandArgs = new string[0];

            if (commandPacket.HasArgs) // TODO: Empacotar em método privado
            {
                this.commandArgs = commandPacket.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 0; i < commandArgs.Length; i++)
            {
                commandArgs[i] = commandArgs[i].Trim();
            }

            this.nextArgPosition = 1; // Default first argument to validate
            this.errorMessages = new List<string>();
            this.commandVariables = new Dictionary<string, object>();
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

        public CommandValidator WithVarBetween<TVarType>(TVarType min, TVarType max, string varName)
        {
            string nextArgumentValue = GetNextArgumentValue();
            if (nextArgumentValue != null)
            {
                var type = typeof(TVarType);
                if (type == typeof(Int32))
                {
                    int nextArgumentValueInt;
                    int minInt = (int)Convert.ChangeType(min, TypeCode.Int32);
                    int maxInt = (int)Convert.ChangeType(max, TypeCode.Int32);
                    bool parseInt = Int32.TryParse(nextArgumentValue, out nextArgumentValueInt);
                    if (parseInt == false || nextArgumentValueInt < minInt || nextArgumentValueInt > maxInt)
                    {
                        this.errorMessages.Add($"{nextArgumentValue} não é um valor válido por não estar entre {min} e {max}");
                    }
                    else if (parseInt == true)
                    {
                        commandVariables.Add(varName, nextArgumentValueInt);
                    }
                }
            }
            else
            {
                this.errorMessages.Add("Informe um valor válido");
            }

            return this;
        }

        public CommandValidator WithVarText(string varName)
        {
            try
            {
                var textValue = commandPacket.Text.Split(new string[] { " " }, this.nextArgPosition + 1, StringSplitOptions.RemoveEmptyEntries)[this.nextArgPosition];
                if (textValue == null)
                {
                    this.errorMessages.Add("Você precisa informar um texto ao final deste comando");
                }
                else
                {
                    this.commandVariables.Add(varName, textValue);
                }
            }
            catch (Exception)
            {
                this.errorMessages.Add("Você precisa informar um texto ao final deste comando.");
            }

            return this;
        }

        public CommandValidator WithVarString(string varName)
        {
            string nextArgumentValue = GetNextArgumentValue();
            if (nextArgumentValue != null)
            {
                commandVariables.Add(varName, nextArgumentValue);
            }
            else
            {
                this.errorMessages.Add("Informe um valor válido");
            }

            return this;
        }

        public void SendCommandError(string errorMessage, string commandSyntax = null)
        {
            this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD2, $"*ARG: {errorMessage}");
            if (commandSyntax != null)
            {
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_WHITE, commandSyntax);
            }
        }

        public bool IsValid(string commandSyntax = null)
        {
            foreach (string errorMessage in this.errorMessages)
            {
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD2, $"*ARG: {errorMessage}");
            }
            if (this.errorMessages.Count > 0 && commandSyntax != null)
            {
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_WHITE, commandSyntax);
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

        public TVarType GetVar<TVarType>(string variableName)
        {
            object variableValue;
            if (commandVariables.TryGetValue(variableName, out variableValue) == false)
            {
                throw new InvalidOperationException($"GetVar<{typeof(TVarType).Name}>() failed to locate variable value for: {variableName}");
            }
            return (TVarType)variableValue;
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