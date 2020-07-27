using CitizenFX.Core;
using GF.CrossCutting;
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

        public void KillPlayer(GFPlayer gfPlayer)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:Kill");
        }

        public void SetPlayerPos(GFPlayer gfPlayer, Vector3 targetPosition)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SetPlayerPos", targetPosition);
        }

        public void TeleportPlayerToPosition(GFPlayer gfPlayer, Vector3 targetPosition, int transitionDurationInMs)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:TeleportPlayerToPosition", targetPosition, transitionDurationInMs);
        }

        public void SetPlayerArmour(GFPlayer gfPlayer, int value)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SetPedArmour", value);
        }

        public void GiveWeaponToPlayer(GFPlayer gfPlayer, WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:GiveWeaponToPed", (uint)weaponHash, ammoCount, isHidden, equipNow);
        }

        public void SetPlayerMoney(GFPlayer gfPlayer, int money)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SetPlayerMoney", money);
        }

        public void OpenMenu(GFPlayer gfPlayer, MenuType menuType)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:OpenMenu", (int)menuType);
        }

        public void SpawnPlayer(GFPlayer gfPlayer, string skinName, float x, float y, float z, float heading)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SpawnPlayer", skinName, x, y, z, heading);
        }

        public void ShowNUIView(GFPlayer gfPlayer, NUIViewType nuiViewType, bool setFocus)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:ShowNUIView", (int)nuiViewType, setFocus);
        }

        public void CloseNUIView(GFPlayer gfPlayer, NUIViewType nuiViewType, bool cancelFocus)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:CloseNUIView", (int)nuiViewType, cancelFocus);
        }
    }
}