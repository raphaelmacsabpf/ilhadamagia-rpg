using System.Collections.Concurrent;

namespace Shared.CrossCutting.Dto
{
    public class PlayerVarsDto : ConcurrentDictionary<string, string>
    {
    }
}