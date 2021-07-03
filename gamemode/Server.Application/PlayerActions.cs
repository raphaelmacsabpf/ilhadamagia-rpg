using CitizenFX.Core;
using Shared.CrossCutting;
using Shared.CrossCutting.Enums;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Managers;
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

        public void KillPlayer(PlayerHandle playerHandle)
        {
            playerHandle.Player.TriggerEvent("GF:Client:Kill");
        }

        public void SetPlayerPos(PlayerHandle playerHandle, Vector3 targetPosition)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SetPlayerPos", targetPosition);
        }

        public void TeleportPlayerToPosition(PlayerHandle playerHandle, Vector3 targetPosition, int transitionDurationInMs)
        {
            playerHandle.Player.TriggerEvent("GF:Client:TeleportPlayerToPosition", targetPosition, transitionDurationInMs);
        }

        public void SetPlayerArmour(PlayerHandle playerHandle, int value)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SetPedArmour", value);
        }

        public void SetPlayerHealth(PlayerHandle playerHandle, int value)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SetPedHealth", value);
        }

        public void GiveWeaponToPlayer(PlayerHandle playerHandle, uint weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            playerHandle.Player.TriggerEvent("GF:Client:GiveWeaponToPed", weaponHash, ammoCount, isHidden, equipNow);
        }

        public void SetPlayerMoney(PlayerHandle playerHandle, int money)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SetPlayerMoney", money);
        }

        public void OpenMenu(PlayerHandle playerHandle, MenuType menuType, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var compressedJson = networkManager.Compress(json);
            var uncompressedLenght = json.Length;
            playerHandle.Player.TriggerEvent("GF:Client:OpenMenu", (int)menuType, compressedJson, uncompressedLenght);
        }

        public void SpawnPlayer(PlayerHandle playerHandle, string skinName, float x, float y, float z, float heading, bool fastSpawn)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SpawnPlayer", skinName, x, y, z, heading, fastSpawn);
        }

        public void OpenNUIView(PlayerHandle playerHandle, NUIViewType nuiViewType, bool setFocus, string compressedJsonPayload, int uncompressedLength)
        {
            playerHandle.Player.TriggerEvent("GF:Client:OpenNUIView", (int)nuiViewType, setFocus, compressedJsonPayload, uncompressedLength);
        }

        public void CreateVehicle(PlayerHandle playerHandle, Vector3 position, float heading, Domain.Entities.Vehicle vehicle)
        {
            playerHandle.Player.TriggerEvent("GF:Client:CreateVehicle", vehicle.Hash, vehicle.PrimaryColor, vehicle.SecondaryColor, vehicle.Fuel, position.X, position.Y, position.Z, heading);
        }

        public void CloseNUIView(PlayerHandle playerHandle, NUIViewType nuiViewType, bool cancelFocus)
        {
            playerHandle.Player.TriggerEvent("GF:Client:CloseNUIView", (int)nuiViewType, cancelFocus);
        }

        public void CreatePlayerVehicle(PlayerHandle playerHandle, GameVehicleHash vehicleHash)
        {
            playerHandle.Player.TriggerEvent("GF:Client:CreatePlayerVehicle", (uint)vehicleHash);
        }

        public void SwitchOutPlayer(PlayerHandle playerHandle)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SwitchOutPlayer");
        }

        public void SwitchInPlayer(PlayerHandle playerHandle, float x, float y, float z)
        {
            playerHandle.Player.TriggerEvent("GF:Client:SwitchInPlayer", x, y, z);
        }
    }
}