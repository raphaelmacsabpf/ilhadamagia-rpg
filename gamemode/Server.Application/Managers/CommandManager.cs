using Server.Application.CommandLibraries;
using Server.Application.Entities;
using Shared.CrossCutting;
using System;
using System.Collections.Generic;

namespace Server.Application.Managers
{
    public class CommandManager
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly CommandLibraryFactory commandLibraryFactory;
        private Dictionary<string, CommandRecord> registeredCommands;

        public CommandManager(ChatManager chatManager, PlayerInfo playerInfo, CommandLibraryFactory commandLibraryFactory)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.commandLibraryFactory = commandLibraryFactory;
            BuildCommandLoader();
            Console.WriteLine("[IM CommandManager] Started CommandManager");
        }

        private void BuildCommandLoader()
        {
            this.registeredCommands = new Dictionary<string, CommandRecord>();
            RegisterCommandsForCommandLibraryType(typeof(AdminCommands));
            RegisterCommandsForCommandLibraryType(typeof(ChatCommands));
            RegisterCommandsForCommandLibraryType(typeof(HouseCommands));
            RegisterCommandsForCommandLibraryType(typeof(MiscCommands));
            RegisterCommandsForCommandLibraryType(typeof(MoneyCommands));
        }

        private void RegisterCommandsForCommandLibraryType(Type type)
        {
            var commandLibraryInstance = this.commandLibraryFactory.GetCommandLibraryInstance(type);
            var commandMethods = commandLibraryInstance.FetchAvailableCommands();
            foreach (var commandMethod in commandMethods)
            {
                if (this.registeredCommands.ContainsKey(commandMethod.Key))
                {
                    var registeredMethod = this.registeredCommands[commandMethod.Key];
                    var registeredDeclaringClassFullName = registeredMethod.MethodInfo.DeclaringType.FullName;
                    var registeredMethodName = registeredMethod.MethodInfo.Name;
                    throw new InvalidOperationException($"Command '{commandMethod.Key}' is already registered for method '{registeredDeclaringClassFullName}.{registeredMethodName}()'");
                }

                var declaringClassFullName = commandMethod.Value.DeclaringType.FullName;
                var methodName = commandMethod.Value.Name;
                this.registeredCommands.Add(commandMethod.Key, new CommandRecord(commandMethod.Value, commandLibraryInstance));
                Console.WriteLine($"Command registered '{commandMethod.Key}' for method '{declaringClassFullName}.{methodName}()'");
            }
        }

        internal void ProcessCommandForPlayer(PlayerHandle sourcePlayerHandle, string command, bool hasArgs, string text)
        {
            CommandPacket commandPacket = new CommandPacket(text);
            commandPacket.Command = command;
            commandPacket.HasArgs = hasArgs;
            commandPacket.Text = text;
            var commandValidator = new CommandValidator(this.playerInfo, this.chatManager, sourcePlayerHandle, commandPacket);

            if (this.registeredCommands.ContainsKey(commandPacket.Command))
            {
                var registeredCommand = this.registeredCommands[commandPacket.Command];
                registeredCommand.MethodInfo.Invoke(registeredCommand.CommandLibraryInstance, new object[] { commandValidator });
            }
            else
            {
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_LIGHTRED, "Comando não reconhecido, use /ajuda para ver alguns comandos!");
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, "Peça ajuda também a um Administrador, use /relatorio");
            }
        }
    }
}