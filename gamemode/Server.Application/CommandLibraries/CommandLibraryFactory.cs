using System;
using System.Collections.Generic;

namespace Server.Application.CommandLibraries
{
    public class CommandLibraryFactory
    {
        private Dictionary<Type, CommandLibrary> commandLibraryInstanceDictionary;

        public void SetAvailableCommandLibraries(Dictionary<Type, CommandLibrary> commandLibraryInstanceDictionary)
        {
            this.commandLibraryInstanceDictionary = commandLibraryInstanceDictionary;
        }

        public CommandLibrary GetCommandLibraryInstance(Type commandLibraryType)
        {
            if (this.commandLibraryInstanceDictionary.ContainsKey(commandLibraryType))
            {
                return this.commandLibraryInstanceDictionary[commandLibraryType];
            }
            else
            {
                throw new KeyNotFoundException($"CommandLibrary '{commandLibraryType.Name}' not defined in AppBootstrap");
            }
        }
    }
}