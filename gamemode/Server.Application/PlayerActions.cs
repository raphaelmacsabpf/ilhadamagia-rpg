using CitizenFX.Core;
using Server.Application.Entities;
using Server.Domain.Enums;
using Shared.CrossCutting;
using System;

namespace Server.Application
{
    public class PlayerActions : BaseScript
    {
        private readonly PlayerInfo playerInfo;

        public PlayerActions(PlayerInfo playerInfo)
        {
            Console.WriteLine("[IM PlayerActions] Started PlayerActions");
            this.playerInfo = playerInfo;
        }

        public void KillPlayer(Player player)
        {
            player.TriggerEvent("GF:Client:Kill");
        }

        public void SetPlayerPos(GFPlayer gfPlayer, Vector3 targetPosition)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SetPlayerPos", targetPosition);
        }

        public void SetPlayerArmour(Player player, int value)
        {
            player.TriggerEvent("GF:Client:SetPedArmour", value);
        }

        public void GiveWeaponToPlayer(Player player, WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            player.TriggerEvent("GF:Client:GiveWeaponToPed", (uint)weaponHash, ammoCount, isHidden, equipNow);
        }

        public void SetPlayerMoney(Player player, int money)
        {
            player.TriggerEvent("GF:Client:SetPlayerMoney", money);
        }

        public void OpenMenu(GFPlayer gfPlayer, MenuType menuType)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:OpenMenu", (int)menuType);
        }

        public void SpawnPlayer(GFPlayer gfPlayer, string skinName, float x, float y, float z, float heading)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SpawnPlayer", skinName, x, y, z, heading);
        }
    }
}