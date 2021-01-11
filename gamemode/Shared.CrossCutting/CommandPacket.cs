using System;

namespace Shared.CrossCutting
{
    public class CommandPacket
    {
        public CommandPacket(string text)
        {
            string[] args = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Command = args[0].ToLower();
            Text = text;
            HasArgs = args.Length > 1;
        }

        public string Command { get; set; }
        public bool HasArgs { get; set; }
        public string Text { get; set; }
    }
}