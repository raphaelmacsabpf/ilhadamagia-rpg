using CitizenFX.Core;
using Server.Application.Entities;
using Server.Application.Managers;
using Shared.CrossCutting;

namespace Server.Application.Services
{
    public class PlayerService
    {
        private readonly ChatManager chatManager;
        private readonly PlayerActions playerActions;

        public PlayerService(ChatManager chatManager, PlayerActions playerActions)
        {
            this.chatManager = chatManager;
            this.playerActions = playerActions;
        }

        public void SetAsAdmin(GFPlayer admin, GFPlayer player, int level)
        {
            player.Account.SetAdminLevel(level);
            this.chatManager.SendClientMessage(player, ChatColor.COLOR_LIGHTBLUE, $"  Você foi promovido a nivel {level} de admin, pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você promoveu {player.Account.Username} para nivel {level} de admin.");
        }

        public void AdminGoToPlayer(GFPlayer admin, GFPlayer player)
        {
            var targetPosition = player.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
            this.playerActions.SetPlayerPos(admin, targetPosition);
            this.chatManager.ProxDetectorColorFixed(10.0f, player, $"*Admin {admin.Account.Username} veio até {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { admin });
            this.chatManager.ProxDetectorColorFixed(10.0f, admin, $"*Admin {admin.Account.Username} foi até {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { player });
        }

        public void AdminBringPlayer(GFPlayer admin, GFPlayer player)
        {
            var sourcePosition = admin.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
            this.playerActions.SetPlayerPos(player, sourcePosition);
            this.chatManager.ProxDetectorColorFixed(10.0f, admin, $"*Admin {admin.Account.Username} trouxe {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { admin });
            this.chatManager.ProxDetectorColorFixed(10.0f, player, $"*Admin {admin.Account.Username} levou {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { player });
        }
    }
}
