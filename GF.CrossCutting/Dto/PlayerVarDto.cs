using System.Collections.Concurrent;

namespace GF.CrossCutting.Dto
{
    public class PlayerVarsDto : ConcurrentDictionary<string, string>
    {
    }
}