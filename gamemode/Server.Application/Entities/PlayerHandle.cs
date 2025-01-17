using CitizenFX.Core;
using GF.CrossCutting.Enums;
using Newtonsoft.Json;
using Server.Application.Enums;
using Server.Application.Managers;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Shared.CrossCutting;
using Shared.CrossCutting.Enums;
using Stateless;
using System;
using System.Collections.Generic;

namespace Server.Application.Entities
{
    public class PlayerHandle
    {
        private House selectedHouse;

        public PlayerHandle(Player player)
        {
            this.Player = player;
            this.License = player.Identifiers["license"];
            this.LicenseAccounts = new List<Account>();
            this.SpawnType = SpawnType.Unset;
            this.IsFirstSpawn = true;
            this.SessionVars = new Dictionary<string, dynamic>();
        }

        public string License { get; }
        public StateMachine<PlayerConnectionState, PlayerConnectionTrigger> FSM { get; set; }
        public Account Account { get; set; }
        public Player Player { get; }

        public House SelectedHouse
        {
            get => selectedHouse;
            set
            {
                selectedHouse = value;
                if (value != null && this.Account != null)
                {
                    this.Account.SetSelectedHouse(value);
                    //HACK: Retirado refatoração DDD|this.Account.SelectedHouse = value.Entity.Id;
                }
            }
        }

        public SpawnType SpawnType { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Vector3 SwitchInPosition { get; set; }
        public bool IsFirstSpawn { get; set; }
        public House CurrentHouseInside { get; set; }
        public List<Account> LicenseAccounts { get; set; }
        public Dictionary<string, dynamic> SessionVars { get; set; }
        public int OrgId { get; set; }

        public void CallClientAction(ClientEvent clientEvent, params object[] args)
        {
            Player.TriggerEvent(clientEvent.ToString(), args);
        }

        public void KillPlayer()
        {
            CallClientAction(ClientEvent.Kill);
        }

        public void SetPlayerPos(Vector3 targetPosition)
        {
            CallClientAction(ClientEvent.SetPlayerPos, targetPosition);
        }

        public void TeleportPlayerToPosition(Vector3 targetPosition, int transitionDurationInMs)
        {
            CallClientAction(ClientEvent.TeleportPlayerToPosition, targetPosition, transitionDurationInMs);
        }

        public void SetPlayerArmour(int value)
        {
            CallClientAction(ClientEvent.SetPedArmour, value);
        }

        public void SetPlayerHealth(int value)
        {
            CallClientAction(ClientEvent.SetPedHealth, value);
        }

        public void GiveWeaponToPlayer(uint weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            CallClientAction(ClientEvent.GiveWeaponToPed, weaponHash, ammoCount, isHidden, equipNow);
        }

        public void SetPlayerMoney(int money)
        {
            CallClientAction(ClientEvent.SetPlayerMoney, money);
        }

        public void OpenMenu(MenuType menuType, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var compressedJson = LZ4Utils.Compress(json);
            var uncompressedLenght = json.Length;
            CallClientAction(ClientEvent.OpenMenu, (int)menuType, compressedJson, uncompressedLenght);
        }

        public void SpawnPlayer(string skinName, float x, float y, float z, float heading, bool fastSpawn)
        {
            CallClientAction(ClientEvent.SpawnPlayer, skinName, x, y, z, heading, fastSpawn);
        }

        public void OpenNUIView(NUIViewType nuiViewType, bool setFocus, string compressedJsonPayload, int uncompressedLength)
        {
            CallClientAction(ClientEvent.OpenNUIView, (int)nuiViewType, setFocus, compressedJsonPayload, uncompressedLength);
        }

        public void CreateVehicle(Vector3 position, float heading, Domain.Entities.Vehicle vehicle)
        {
            CallClientAction(ClientEvent.CreateVehicle, vehicle.Hash, vehicle.PrimaryColor, vehicle.SecondaryColor, vehicle.Fuel, position.X, position.Y, position.Z, heading);
        }

        public void CloseNUIView(NUIViewType nuiViewType, bool cancelFocus)
        {
            CallClientAction(ClientEvent.CloseNUIView, (int)nuiViewType, cancelFocus);
        }

        public void CreatePlayerVehicle( GameVehicleHash vehicleHash)
        {
            CallClientAction(ClientEvent.CreatePlayerVehicle, (uint)vehicleHash);
        }

        public void SwitchOutPlayer()
        {
            CallClientAction(ClientEvent.SwitchOutPlayer);
        }

        public void SwitchInPlayer(float x, float y, float z)
        {
            CallClientAction(ClientEvent.SwitchInPlayer, x, y, z);
        }

        public void SendPayloadToPlayer(PayloadType payloadType, string jsonPayload)
        {
            var compressedJsonPayload = LZ4Utils.Compress(jsonPayload);
            CallClientAction(ClientEvent.SendPayload, (int)payloadType, compressedJsonPayload, jsonPayload.Length);
        }

        public void SyncPlayerDateTime()
        {
            CallClientAction(ClientEvent.SyncPlayerDateTime, DateTime.Now.ToString());
        }

        internal void OrgEquip()
        {
            throw new NotImplementedException();
        }
    }
}