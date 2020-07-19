using CitizenFX.Core;
using GF.CrossCutting;
using Server.Entities;
using Server.Enums;
using System;

namespace Server
{
    public class PlayerActions : BaseScript
    {
        public PlayerActions(bool ignoreFiveMStart)
        {
            Console.WriteLine("[IM PlayerActions] Started PlayerActions");
        }

        public void KillPlayer(Player player)
        {
            player.TriggerEvent("GF:Client:Kill");
        }

        public void SetPlayerPos(Player player, Vector3 targetPosition)
        {
            player.TriggerEvent("GF:Client:SetPlayerPos", targetPosition);
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
    }
}