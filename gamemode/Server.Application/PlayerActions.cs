﻿using CitizenFX.Core;
using Shared.CrossCutting;
using Server.Domain.Entities;
using Server.Domain.Enums;
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
            var player = playerInfo.GetPlayer(gfPlayer);
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
            playerInfo.GetPlayer(gfPlayer).TriggerEvent("GF:Client:OpenMenu", (int)menuType);
        }
    }
}