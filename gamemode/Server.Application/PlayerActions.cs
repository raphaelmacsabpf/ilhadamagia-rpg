﻿using CitizenFX.Core;
using GF.CrossCutting;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Managers;
using Server.Domain.Enums;
using Shared.CrossCutting;
using System;

namespace Server.Application
{
    public class PlayerActions : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;

        public PlayerActions(PlayerInfo playerInfo, NetworkManager networkManager)
        {
            Console.WriteLine("[IM PlayerActions] Started PlayerActions");
            this.playerInfo = playerInfo;
            this.networkManager = networkManager;
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

        public void OpenMenu(GFPlayer gfPlayer, MenuType menuType, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var compressedJson = networkManager.Compress(json);
            var uncompressedLenght = json.Length;
            gfPlayer.Player.TriggerEvent("GF:Client:OpenMenu", (int)menuType, compressedJson, uncompressedLenght);
        }

        public void SpawnPlayer(GFPlayer gfPlayer, string skinName, float x, float y, float z, float heading)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SpawnPlayer", skinName, x, y, z, heading);
        }

        public void OpenNUIView(GFPlayer gfPlayer, NUIViewType nuiViewType, bool setFocus, string compressedJsonPayload, int uncompressedLength)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:OpenNUIView", (int)nuiViewType, setFocus, compressedJsonPayload, uncompressedLength);
        }

        public void CreateVehicle(GFPlayer gfPlayer, Vector3 position, float heading, Domain.Entities.Vehicle vehicle)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:CreateVehicle", vehicle.Hash, vehicle.PrimaryColor, vehicle.SecondaryColor, vehicle.Fuel, position.X, position.Y, position.Z, heading);
        }

        public void CloseNUIView(GFPlayer gfPlayer, NUIViewType nuiViewType, bool cancelFocus)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:CloseNUIView", (int)nuiViewType, cancelFocus);
        }
    }
}