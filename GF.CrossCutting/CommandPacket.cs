namespace Shared.CrossCutting
{
    public class CommandPacket
    {
        public CommandCode CommandCode { get; set; }
        public bool HasArgs { get; set; }
        public string Text { get; set; }
    }
}