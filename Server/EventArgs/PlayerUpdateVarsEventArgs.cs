using System;

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