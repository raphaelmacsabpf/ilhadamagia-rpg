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
        private readonly PlayerHandle sourcePlayerHandle;
        private readonly CommandPacket commandPacket;
        private PlayerHandle targetPlayerHandle;
        private readonly string[] commandArgs;
        private readonly List<string> variableNames;
        private int nextArgPosition;
        private readonly List<string> errorMessages;
        private readonly Dictionary<string, object> commandVariables;

        public CommandValidator(PlayerInfo playerInfo, ChatManager chatManager, PlayerHandle playerHandle, CommandPacket commandPacket)
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.sourcePlayerHandle = playerHandle;
            this.commandPacket = commandPacket;
            this.commandArgs = new string[0];
            this.variableNames = new List<string>();

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

        public PlayerHandle SourcePlayerHandle { get => this.sourcePlayerHandle; }
        public CommandPacket CommandPacket { get => this.commandPacket; }

        public CommandValidator WithAdminLevel(int minAdminLevel)
        {
            if (this.sourcePlayerHandle.Account.AdminLevel < minAdminLevel)
            {
                this.errorMessages.Add("Você não está autorizado a utilizar este comando");
            }
            return this;
        }

        public CommandValidator WithTargetPlayer(string varName)
        {
            string nextArgumentValue = GetNextArgumentValue(varName);
            if (nextArgumentValue != null)
            {
                this.targetPlayerHandle = GetPlayerByIdOrName(nextArgumentValue);
                if (this.targetPlayerHandle == null)
                {
                    this.errorMessages.Add($"{nextArgumentValue} não é um {varName} válido");
                }
            }
            else
            {
                this.errorMessages.Add($"Informe um {varName} válido");
            }

            return this;
        }

        public CommandValidator WithVarBetween<TVarType>(TVarType min, TVarType max, string varName)
        {
            string nextArgumentValue = GetNextArgumentValue(varName);
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
                        this.errorMessages.Add($"{nextArgumentValue} não é um {varName} válido por não estar entre {min} e {max}");
                    }
                    else if (parseInt == true)
                    {
                        commandVariables.Add(varName, nextArgumentValueInt);
                    }
                }
                else if (type == typeof(Int64))
                {
                    long nextArgumentValueLong;
                    long minLong = (long)Convert.ChangeType(min, TypeCode.Int64);
                    long maxLong = (long)Convert.ChangeType(max, TypeCode.Int64);
                    bool parseLong = Int64.TryParse(nextArgumentValue, out nextArgumentValueLong);
                    if (parseLong == false || nextArgumentValueLong < minLong || nextArgumentValueLong > maxLong)
                    {
                        this.errorMessages.Add($"{nextArgumentValue} não é um {varName} válido por não estar entre {min} e {max}");
                    }
                    else if (parseLong == true)
                    {
                        commandVariables.Add(varName, nextArgumentValueLong);
                    }
                }
            }
            else
            {
                this.errorMessages.Add($"Informe um {varName} válido");
            }

            return this;
        }

        public CommandValidator WithVarText(string varName)
        {
            try
            {
                var textValue = commandPacket.Text.Split(new string[] { " " }, this.nextArgPosition + 1, StringSplitOptions.RemoveEmptyEntries)[this.nextArgPosition];
                this.commandVariables.Add(varName, textValue);
            }
            catch (Exception)
            {
                this.errorMessages.Add("Você precisa informar um texto ao final deste comando.");
            }

            return this;
        }

        public CommandValidator WithVarString(string varName)
        {
            string nextArgumentValue = GetNextArgumentValue(varName);
            if (nextArgumentValue != null)
            {
                commandVariables.Add(varName, nextArgumentValue);
            }
            else
            {
                this.errorMessages.Add($"Informe um {varName} válido");
            }

            return this;
        }

        public void SendCommandError(string errorMessage, string commandSyntax = null)
        {
            this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_GRAD2, $"*ARG: {errorMessage}");
            if (commandSyntax != null)
            {
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_WHITE, commandSyntax);
            }
        }

        public bool IsValid(string commandSyntax = null)
        {
            for (var i = 0; i < this.errorMessages.Count; i++)
            {
                var errorMessage = this.errorMessages[i];
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_GRAD2, $"*ARG [{variableNames[i]}]: {errorMessage}");
            }

            if (this.errorMessages.Count > 0 && commandSyntax != null)
            {
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_WHITE, commandSyntax);
            }
            this.nextArgPosition = 1;
            return this.errorMessages.Count == 0;
        }

        public PlayerHandle GetTargetPlayerHandle()
        {
            if (this.targetPlayerHandle == null)
            {
                throw new InvalidOperationException("GetTargetPlayerHandle() called without TargetPlayer validation");
            }
            return this.targetPlayerHandle;
        }

        public TVarType GetVar<TVarType>(string variableName)
        {
            if (commandVariables.TryGetValue(variableName, out var variableValue) == false)
            {
                throw new InvalidOperationException($"GetVar<{typeof(TVarType).Name}>() failed to locate variable value for: {variableName}");
            }
            return (TVarType)variableValue;
        }

        private string GetNextArgumentValue(string varName)
        {
            this.variableNames.Add(varName);
            if (commandArgs.Length > nextArgPosition)
            {
                var nextArgumentValue = commandArgs[nextArgPosition];
                nextArgPosition++;
                return nextArgumentValue;
            }
            return null;
        }

        private PlayerHandle GetPlayerByIdOrName(string playerStr)
        {
            bool parseSucceed = Int32.TryParse(playerStr, out var parsedId);
            var playerHandleList = this.playerInfo.GetPlayerHandleList();
            if (parseSucceed)
            {
                foreach (var playerHandle in playerHandleList)
                {
                    if (Int32.Parse(playerHandle.Player.Handle) == parsedId)
                    {
                        return playerHandle;
                    }
                }
            }
            else
            {
                var loweredPlayerStr = playerStr.ToLower();
                foreach (var playerHandle in playerHandleList)
                {
                    var loweredUsername = playerHandle.Account.Username.ToLower();
                    if (loweredUsername.StartsWith(loweredPlayerStr) || loweredUsername == loweredPlayerStr)
                    {
                        return playerHandle;
                    }
                }
            }

            return null;
        }
    }
}