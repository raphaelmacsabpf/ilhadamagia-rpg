using CitizenFX.Core;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Managers;
using Server.Domain.Enums;
using Shared.CrossCutting;

namespace Server.Application.Services
{
    public class PlayerService
    {
        private readonly ChatManager chatManager;
        private readonly PlayerActions playerActions;
        private readonly MapManager mapManager;

        public PlayerService(ChatManager chatManager, PlayerActions playerActions, MapManager mapManager)
        {
            this.chatManager = chatManager;
            this.playerActions = playerActions;
            this.mapManager = mapManager;
        }

        public void SetAsAdmin(PlayerHandle admin, PlayerHandle player, int level)
        {
            player.Account.SetAdminLevel(level);
            this.chatManager.SendClientMessage(player, ChatColor.COLOR_LIGHTBLUE, $"  Você foi promovido a nivel {level} de admin, pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você promoveu {player.Account.Username} para nivel {level} de admin.");
        }

        public void AdminGoToPlayer(PlayerHandle admin, PlayerHandle player)
        {
            var targetPosition = player.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
            this.playerActions.SetPlayerPos(admin, targetPosition);
            this.chatManager.ProxDetectorColorFixed(10.0f, player, $"*Admin {admin.Account.Username} veio até {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { admin });
            this.chatManager.ProxDetectorColorFixed(10.0f, admin, $"*Admin {admin.Account.Username} foi até {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { player });
        }

        public void AdminBringPlayer(PlayerHandle admin, PlayerHandle player)
        {
            var sourcePosition = admin.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
            this.playerActions.SetPlayerPos(player, sourcePosition);
            this.chatManager.ProxDetectorColorFixed(10.0f, admin, $"*Admin {admin.Account.Username} trouxe {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { admin });
            this.chatManager.ProxDetectorColorFixed(10.0f, player, $"*Admin {admin.Account.Username} levou {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { player });
        }

        internal void SetPlayerHealth(PlayerHandle admin, PlayerHandle targetPlayer, int value)
        {
            this.playerActions.SetPlayerHealth(targetPlayer, value);
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de saúde para {targetPlayer.Account.Username}");
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetPlayer.Account.Username} te deu {value} de saúde");
        }

        internal void SetPlayerArmour(PlayerHandle admin, PlayerHandle targetPlayer, int value)
        {
            this.playerActions.SetPlayerArmour(targetPlayer, value);
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de colete para {targetPlayer.Account.Username}");
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetPlayer.Account.Username} te deu {value} de colete");
        }

        internal void SetPlayerPos(PlayerHandle targetPlayer, Vector3 position)
        {
            this.playerActions.SetPlayerPos(targetPlayer, position);
        }

        internal void SetPlayerSelectedHouse(PlayerHandle admin, PlayerHandle targetPlayer, int houseId)
        {
            targetPlayer.SelectedHouse = mapManager.GetGFHouseFromId(houseId);
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você setou a casa de {targetPlayer.Account.Username} para ID {houseId}.");
        }

        internal void TeleportPlayerToHouse(PlayerHandle targetPlayer, int houseId)
        {
            var gfHouse = mapManager.GetGFHouseFromId(houseId);
            this.playerActions.TeleportPlayerToPosition(targetPlayer, new Vector3(gfHouse.EntranceX, gfHouse.EntranceY, gfHouse.EntranceZ), 500);
        }

        internal void SetSkin(PlayerHandle admin, PlayerHandle targetPlayer, string pedModelHash)
        {
            targetPlayer.Account.SetPedModel(pedModelHash);
            targetPlayer.FSM.Fire(PlayerConnectionTrigger.SET_TO_SPAWN);
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $" Sua skin foi alterada pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você alterou a skin de {targetPlayer.Account.Username}");
        }

        internal void SpawnPlayer(PlayerHandle player)
        {
            this.playerActions.SwitchInPlayer(player, player.SwitchInPosition.X, player.SwitchInPosition.Y, player.SwitchInPosition.Z);
            var fastSpawn = player.SpawnType == SpawnType.ToCoords;
            playerActions.SpawnPlayer(player, player.Account.PedModel, player.SpawnPosition.X, player.SpawnPosition.Y, player.SpawnPosition.Z, 0, fastSpawn);
            player.SpawnType = SpawnType.Unset;
        }
    }
}
