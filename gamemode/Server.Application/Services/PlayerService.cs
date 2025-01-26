using CitizenFX.Core;
using CitizenFX.Core.Native;
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
        private readonly MapManager mapManager;

        public PlayerService(ChatManager chatManager, MapManager mapManager)
        {
            this.chatManager = chatManager;
            this.mapManager = mapManager;
        }

        public void SetPlayerDimension(PlayerHandle playerHandle, int dimension)
        {
            var SET_PLAYER_ROUTING_BUCKET = (Hash)0x6504EB38;
            Function.Call(SET_PLAYER_ROUTING_BUCKET, playerHandle.Player.Handle.ToString(), dimension);
        }

        public int GetPlayerDimension(PlayerHandle playerHandle)
        {
            var GET_PLAYER_ROUTING_BUCKET = (Hash)0x52441C34;
            var dimension = Function.Call<int>(GET_PLAYER_ROUTING_BUCKET, playerHandle.Player.Handle.ToString());
            return dimension;
        }

        public void SetAsAdmin(PlayerHandle admin, PlayerHandle player, int level)
        {
            player.Account.SetAdminLevel(level);
            this.chatManager.SendClientMessage(player, ChatColor.COLOR_LIGHTBLUE, $"  Você foi promovido a nivel {level} de admin, pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você promoveu {player.Account.Username} para nivel {level} de admin.");
        }

        public void SetMaxAdmin(PlayerHandle admin, PlayerHandle player, int level)
        {
            player.Account.SetMaxAdminLevel(level);
            this.chatManager.SendClientMessage(player, ChatColor.COLOR_LIGHTBLUE, $"  Seu nível de admin máximo foi ajustado, pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você ajustou o nível de admin máximo de {player.Account.Username} para nivel {level}.");
        }

        public void AdminGoToPlayer(PlayerHandle admin, PlayerHandle player)
        {
            var targetPosition = player.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
            admin.SetPlayerPos(targetPosition);
            this.chatManager.ProxDetectorColorFixed(10.0f, player, $"*Admin {admin.Account.Username} veio até {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { admin });
            this.chatManager.ProxDetectorColorFixed(10.0f, admin, $"*Admin {admin.Account.Username} foi até {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { player });
        }

        public void AdminBringPlayer(PlayerHandle admin, PlayerHandle player)
        {
            var sourcePosition = admin.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
            player.SetPlayerPos(sourcePosition);
            this.chatManager.ProxDetectorColorFixed(10.0f, admin, $"*Admin {admin.Account.Username} trouxe {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { admin });
            this.chatManager.ProxDetectorColorFixed(10.0f, player, $"*Admin {admin.Account.Username} levou {player.Account.Username}", ChatColor.COLOR_PURPLE, new[] { player });
        }

        internal void SetPlayerHealth(PlayerHandle admin, PlayerHandle targetPlayer, int value)
        {
            targetPlayer.SetPlayerHealth(value);
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de saúde para {targetPlayer.Account.Username}");
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetPlayer.Account.Username} te deu {value} de saúde");
        }

        internal void SetPlayerArmour(PlayerHandle admin, PlayerHandle targetPlayer, int value)
        {
            targetPlayer.SetPlayerArmour(value);
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de colete para {targetPlayer.Account.Username}");
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetPlayer.Account.Username} te deu {value} de colete");
        }

        internal void SetPlayerPos(PlayerHandle targetPlayer, Vector3 position)
        {
            targetPlayer.SetPlayerPos(position);
        }

        internal void SetPlayerSelectedHouse(PlayerHandle admin, PlayerHandle targetPlayer, int houseId)
        {
            targetPlayer.SelectedHouse = mapManager.GetGFHouseFromId(houseId);
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você setou a casa de {targetPlayer.Account.Username} para ID {houseId}.");
        }

        internal void TeleportPlayerToHouse(PlayerHandle targetPlayer, int houseId)
        {
            var gfHouse = mapManager.GetGFHouseFromId(houseId);
            targetPlayer.TeleportPlayerToPosition(new Vector3(gfHouse.EntranceX, gfHouse.EntranceY, gfHouse.EntranceZ), 500);
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
            player.SwitchInPlayer(player.SwitchInPosition.X, player.SwitchInPosition.Y, player.SwitchInPosition.Z);
            var fastSpawn = player.SpawnType == SpawnType.ToCoords;
            player.SpawnPlayer(player.Account.PedModel, player.SpawnPosition.X, player.SpawnPosition.Y, player.SpawnPosition.Z, 0, fastSpawn);
            player.SpawnType = SpawnType.Unset;
            this.SetPlayerDimension(player, player.SpawnDimension);
        }
    }
}
