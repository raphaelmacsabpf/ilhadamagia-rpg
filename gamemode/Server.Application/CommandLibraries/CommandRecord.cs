using System.Reflection;

namespace Server.Application.CommandLibraries
{
    public class CommandRecord
    {
        public CommandRecord(MethodInfo methodInfo, CommandLibrary commandLibraryInstance)
        {
            MethodInfo = methodInfo;
            CommandLibraryInstance = commandLibraryInstance;
        }

        public MethodInfo MethodInfo { get; }
        public CommandLibrary CommandLibraryInstance { get; }
    }
}
