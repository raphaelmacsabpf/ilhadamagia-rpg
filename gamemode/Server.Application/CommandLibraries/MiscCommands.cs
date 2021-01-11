using Server.Application.Managers;
using Shared.CrossCutting;
using System.Linq;

namespace Server.Application.CommandLibraries
{
    public class MiscCommands : CommandLibrary
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;

        public MiscCommands(ChatManager chatManager, PlayerInfo playerInfo)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
        }

        [Command("/admins")]
        public void AdminList(CommandValidator commandValidator)
        {
            var playerList = this.playerInfo.GetGFPlayerList();
            var admins = playerList.Where((gfPlayer) => gfPlayer.Account.AdminLevel > 0);

            chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_ROSA, "Administradores online:");

            foreach (var admin in admins)
            {
                string adminRank;
                switch (admin.Account.AdminLevel)
                {
                    case 1337:
                        adminRank = "MASTER"; break;
                    case 3001:
                        adminRank = ""; break;
                    case 1:
                        adminRank = "HELPER"; break;
                    default:
                        adminRank = $"Nível {admin.Account.AdminLevel}"; break;
                }

                if (adminRank != "")
                {
                    chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD1, $"Admin: {admin.Account.Username} [{adminRank}]");
                }
            }
        }
    }
}