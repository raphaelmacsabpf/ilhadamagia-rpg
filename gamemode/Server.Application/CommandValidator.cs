using Server.Application.Entities;

namespace Server.Application
{
    public class CommandValidator
    {
        private GFPlayer gfPlayer;
        private bool hasErrors;
        public CommandValidator(GFPlayer gfPlayer)
        {
            this.gfPlayer = gfPlayer;
        }

        public CommandValidator AdminLevel(int minAdminLevel)
        {
            this.hasErrors |= this.gfPlayer.Account.AdminLevel < minAdminLevel;
            return this;
        }

        public bool IsInvalid()
        {
            return this.hasErrors;
        }
    }
}
