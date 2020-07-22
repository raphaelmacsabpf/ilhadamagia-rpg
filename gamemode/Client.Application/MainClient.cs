﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Client.Application
{
    public class MainClient : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly MarkersManager markersManager;
        private readonly Render render;
        private readonly PlayerActions playerActions;
        private readonly TargetsManager targetsManager;
        private readonly MenuManager menuManager;

        public MainClient(PlayerInfo playerInfo, MarkersManager markersManager, Render render, PlayerActions playerActions, TargetsManager targetsManager, MenuManager menuManager)
        {
            API.RemoveMultiplayerWalletCash();
            API.RemoveMultiplayerHudCash();

            this.playerInfo = playerInfo;
            this.markersManager = markersManager;
            this.render = render;
            this.playerActions = playerActions;
            this.targetsManager = targetsManager;
            this.menuManager = menuManager;
        }

        public void GFDeleteVehicle(int vehicleHandle)
        {
            Debug.WriteLine("--------------- HANDLE DO VEH É: " + vehicleHandle);
            int hndl = vehicleHandle;
            API.DeleteVehicle(ref hndl);
        }

        public void GFSetPlayerMoney(int money)
        {
            render.UpdateMoney(this.playerInfo.Money, money);
            this.playerInfo.Money = money;
        }

        public async void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;

            //await Delay(20000);
            TriggerServerEvent("GF:Server:OnClientReady");
        }

        public void OnPayloadReceive(int payloadTypeInt, string payload)
        {
            var subStringLenght = payload.Length < 80 ? payload.Length : payload.Length;
            Debug.WriteLine($"Payload[{payload.Length}]: { payload.Substring(0, subStringLenght)}..."); // TODO: Remove before release
            PayloadType payloadType = (PayloadType)payloadTypeInt;
            switch (payloadType)
            {
                case PayloadType.TO_STATIC_MARKERS:
                    {
                        this.markersManager.Markers = JsonConvert.DeserializeObject<List<MarkerDto>>(payload);
                        return;
                    }
                case PayloadType.TO_PLAYER_VARS:
                    {
                        var PlayerVar = JsonConvert.DeserializeObject<PlayerVarsDto>(payload);
                        foreach (var playerVar in PlayerVar)
                        {
                            switch (playerVar.Key)
                            {
                                case "Money":
                                    {
                                        var money = Int32.Parse(playerVar.Value);
                                        GFSetPlayerMoney(money);
                                        break;
                                    }
                            }
                        }
                        return;
                    }
                case PayloadType.TO_STATIC_PROXIMITY_TARGETS:
                    {
                        this.targetsManager.ProximityTargets = JsonConvert.DeserializeObject<List<ProximityTargetDto>>(payload);
                        return;
                    }
                case PayloadType.TO_STATIC_INTERACTION_TARGETS:
                    {
                        this.targetsManager.InteractionTargets = JsonConvert.DeserializeObject<List<InteractionTargetDto>>(payload);
                        return;
                    }
            }
        }

        public void OnClienText(string textInput)
        {
            API.CancelEvent();
            if (textInput[0] == '/')
            {
                var commandPacket = CommandParser.Parse(textInput);
                TriggerServerEvent("GF:Server:OnClientCommand", (int)commandPacket.CommandCode, commandPacket.HasArgs, commandPacket.Text);
            }
            else
                TriggerServerEvent("GF:Server:OnChatMessage", textInput);
        }

        // Default Events
        public void OnPlayerSpawn(object obj)
        {
            API.CancelEvent();

            // TODO: Improve!!! Client has to send spawn event to server.
            // Default SPAWN
            playerActions.SpawnPlayer("S_M_Y_MARINE_01", 309.6f, -728.7297f, 29.3136f, 246.6142f);
        }

        public async void OnDie(int killerType, dynamic deathCoords)
        {
            API.CancelEvent();
            await Spawn.SpawnPlayer("S_M_Y_MARINE_01", 309.6f, -728.7297f, 29.3136f, 246.6142f);
        }

        public async void OnPlayerMapStart()
        {
            API.CancelEvent();

            API.RequestModel((uint)PedHash.Marine01SMY);
            while (API.HasModelLoaded((uint)PedHash.Marine01SMY) == false)
            {
                await Delay(300);
            }
            await Spawn.SpawnPlayer("S_M_Y_MARINE_01", 309.6f, -728.7297f, 29.3136f, 246.6142f);
        }

        // Commands
        public void OnInfo() // TODO: REMOVE THIS COMMAND
        {
            Screen.ShowNotification($"~b~ServerInfo~s~: Your are currently on the Tutorial Server By 4444, {Game.Player.Name}!");
        }

        public async void onVeh() // TODO: Add complete /veh command
        {
            for (int i = 0; i < 1; i++)
            {
                var model = new Model(VehicleHash.Infernus);
                // create the vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                // set the player ped into the vehicle and driver seat
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);
                // tell the player
                this.playerActions.SendMessageToPlayerChat(ChatColor.COLOR_GRAD1, $"Woohoo! Enjoy your new ^*{model}!");
                await Delay(250);
            }
        }

        // GF Functions
        public void GFSendClientMessage(int chatColor, string message)
        {
            this.playerActions.SendMessageToPlayerChat((ChatColor)chatColor, message);
        }

        public async void GFSpawnPlayer(string skinName, float x, float y, float z, float heading)
        {
            this.playerActions.SpawnPlayer(skinName, x, y, z, heading);
        }

        public void GFKill()
        {
            this.playerActions.Kill();
        }

        public void GFSetPlayerPos(Vector3 targetPosition)
        {
            this.playerActions.SetPlayerPos(targetPosition);
        }

        public void GFGiveWeaponToPed(uint weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            this.playerActions.GivePlayerWeapon((WeaponHash)weaponHash, ammoCount, isHidden, equipNow);
        }

        public void GFSetPedArmour(int value)
        {
            this.playerActions.SetPlayerArmour(value);
        }
    }
}

// SOUNDS

//API.PlaySound(-1, "CANCEL", "HUD_MINI_GAME_SOUNDSET", false, 0, true);
///////API.PlaySound(-1, "CANCEL", sub_82c23(), 0, 0, 1);
//------API.PlaySound(-1, "CHARACTER_CHANGE_CHARACTER_01_MASTER", "0", false, 0, false);
//API.PlaySound(-1, "CHARACTER_CHANGE_UP_MASTER", 0, 0, 0, 1);
///////////API.PlaySound(-1, "NAV_UP_DOWN", sub_82c23(), 0, 0, 1);
//++++API.PlaySound(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false, 0, true);
//++++API.PlaySound(-1, "SELECT", "HUD_MINI_GAME_SOUNDSET", false, 0, true);
//////////////API.PlaySound(-1, "SELECT", sub_82c23(), 0, 0, 1);
//++++API.PlaySound(-1, "slow", "SHORT_PLAYER_SWITCH_SOUND_SET", false, 0, true);
//------API.PlaySound(-1, "Take_Picture", "MUGSHOT_CHARACTER_CREATION_SOUNDS", false, 0, true);
//------API.PlaySound(-1, "Virus_Eradicated", "LESTER1A_SOUNDS", false, 0, true);
//------API.PlaySound(-1, "Zoom_In", "MUGSHOT_CHARACTER_CREATION_SOUNDS", false, 0, true);
//API.PlaySound(-1, "Zoom_Out", "MUGSHOT_CHARACTER_CREATION_SOUNDS", false, 0, true);

// cachorro louco ataca player ped
//var position = Game.PlayerPed.Position;
//API.RequestModel((uint)PedHash.Chop);
//while (API.HasModelLoaded((uint)PedHash.Chop) == false)
//{
//    await Delay(300);
//}
//var handle = API.CreatePed(2, (uint)PedHash.Chop, position.X, position.Y, position.Z, 90.0f, true, true);
////API.SetPedAsEnemy(handle, true);
///*API.SetPedAsEnemy(handle, true);
//API.ExplodePedHead(handle, (uint)WeaponHash.Minigun);
//API.SetCanPedEquipAllWeapons(handle, true);
//API.SetPedDefensiveSphereAttachedToPed(handle, Game.PlayerPed.Handle, 0, 0, 0, 10, true);
//API.SetPedHairColor(handle, 1, 2);
//*/
//API.SetPedCombatAttributes(handle, 46, true);
//API.SetPedFleeAttributes(handle, 0, false);
//API.SetPedAsEnemy(handle, true);
//API.SetPedRelationshipGroupHash(handle, (uint)API.GetHashKey("HATES_PLAYER"));
////API.SetPedAsCop(handle, true);

////API.GiveWeaponToPed(handle, (uint)WeaponHash.AssaultSMG, 300, false, true);

////API.SetPedAmmo(handle, (uint)WeaponHash.AssaultSMG, 300);

//GFSendClientMessage((int)ChatColor.COLOR_LIGHTRED, $"Foi criado PED com handle: {handle}");