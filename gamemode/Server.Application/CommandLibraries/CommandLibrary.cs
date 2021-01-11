using System.Collections.Generic;
using System.Reflection;

namespace Server.Application.CommandLibraries
{
    public abstract class CommandLibrary
    {
        public IEnumerable<KeyValuePair<string, MethodInfo>> FetchAvailableCommands()
        {
            var exposedCommandsMap = new List<KeyValuePair<string, MethodInfo>>();
            var methods = GetType().GetMethods();
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<CommandAttribute>();
                if (attribute != null)
                {
                    exposedCommandsMap.Add(new KeyValuePair<string, MethodInfo>(attribute.Command, method));
                }
            }
            return exposedCommandsMap;
        }
    }
}