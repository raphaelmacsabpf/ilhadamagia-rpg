using GF.CrossCutting;

namespace Client
{
    public static class CommandParser
    {
        public static CommandPacket Parse(string text)
        {
            var commandPacket = new CommandPacket();

            string[] args = text.Split(' ');
            var command = args[0].ToLower();

            commandPacket.Text = text;
            commandPacket.CommandCode = GetCommandCode(command);
            commandPacket.HasArgs = args.Length > 1;

            return commandPacket;
        }

        private static CommandCode GetCommandCode(string command)
        {
            switch (command)
            {
                case "/gritar": case "/g": return CommandCode.SCREAM;
                case "/info": return CommandCode.INFO;
                case "/veh": return CommandCode.VEH;
                case "/kill": return CommandCode.KILL;
                case "/ir": return CommandCode.GO;
                case "/trazer": return CommandCode.BRING;
                case "/save": return CommandCode.SAVE;
                case "/setadmin": return CommandCode.SET_ADMIN;
                case "/setcolete": return CommandCode.SET_ARMOUR;
                case "/coords": return CommandCode.GO_TO_COORDS;
                case "/prop": return CommandCode.PROP_MENU;
                case "/setcasa": return CommandCode.SET_HOUSE;
                default: return CommandCode.INVALID_COMMAND;
            }
        }
    }
}