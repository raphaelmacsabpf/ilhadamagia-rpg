using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class PlayerUpdateVarsEventArgs : EventArgs
    {
        public PlayerUpdateVarsEventArgs(string playerVar, string value)
        {
            PlayerVar = playerVar;
            Value = value;
        }

        public string PlayerVar { get; }
        public string Value { get; }
    }
}
