using CitizenFX.Core;
using Shared.CrossCutting;
using Shared.CrossCutting.Enums;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Managers;
using System;
using GF.CrossCutting.Enums;

namespace Server.Application
{
    public class PlayerActions : BaseScript
    {
        private readonly NetworkManager networkManager;

        public PlayerActions(NetworkManager networkManager)
        {
            Console.WriteLine("[IM PlayerActions] Started PlayerActions");
            this.networkManager = networkManager;
        }

        public void KillPlayer(PlayerHandle playerHandle)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.Kill);
        }

        public void SetPlayerPos(PlayerHandle playerHandle, Vector3 targetPosition)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SetPlayerPos, targetPosition);
        }

        public void TeleportPlayerToPosition(PlayerHandle playerHandle, Vector3 targetPosition, int transitionDurationInMs)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.TeleportPlayerToPosition, targetPosition, transitionDurationInMs);
        }

        public void SetPlayerArmour(PlayerHandle playerHandle, int value)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SetPedArmour, value);
        }

        public void SetPlayerHealth(PlayerHandle playerHandle, int value)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SetPedHealth, value);
        }

        public void GiveWeaponToPlayer(PlayerHandle playerHandle, uint weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.GiveWeaponToPed, weaponHash, ammoCount, isHidden, equipNow);
        }

        public void SetPlayerMoney(PlayerHandle playerHandle, int money)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SetPlayerMoney, money);
        }

        public void OpenMenu(PlayerHandle playerHandle, MenuType menuType, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var compressedJson = networkManager.Compress(json);
            var uncompressedLenght = json.Length;
            playerHandle.TriggerScriptEvent(ScriptEvent.OpenMenu, (int)menuType, compressedJson, uncompressedLenght);
        }

        public void SpawnPlayer(PlayerHandle playerHandle, string skinName, float x, float y, float z, float heading, bool fastSpawn)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SpawnPlayer, skinName, x, y, z, heading, fastSpawn);
        }

        public void OpenNUIView(PlayerHandle playerHandle, NUIViewType nuiViewType, bool setFocus, string compressedJsonPayload, int uncompressedLength)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.OpenNUIView, (int)nuiViewType, setFocus, compressedJsonPayload, uncompressedLength);
        }

        public void CreateVehicle(PlayerHandle playerHandle, Vector3 position, float heading, Domain.Entities.Vehicle vehicle)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.CreateVehicle, vehicle.Hash, vehicle.PrimaryColor, vehicle.SecondaryColor, vehicle.Fuel, position.X, position.Y, position.Z, heading);
        }

        public void CloseNUIView(PlayerHandle playerHandle, NUIViewType nuiViewType, bool cancelFocus)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.CloseNUIView, (int)nuiViewType, cancelFocus);
        }

        public void CreatePlayerVehicle(PlayerHandle playerHandle, GameVehicleHash vehicleHash)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.CreatePlayerVehicle, (uint)vehicleHash);
        }

        public void SwitchOutPlayer(PlayerHandle playerHandle)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SwitchOutPlayer);
        }

        public void SwitchInPlayer(PlayerHandle playerHandle, float x, float y, float z)
        {
            playerHandle.TriggerScriptEvent(ScriptEvent.SwitchInPlayer, x, y, z);
        }
    }
}