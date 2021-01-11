using System;

namespace Server.Application.CommandLibraries
{
    class CommandAttribute : Attribute
    {
        public CommandAttribute(string command)
        {
            Command = command;
        }

        public string Command { get; }
    }
}
