using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using GF.CrossCutting;
using Newtonsoft.Json;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
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
        private readonly ClientNetworkManager clientNetworkManager;
        private bool clientInitializationStarted;
        private string lastPayloadCompressed; // TODO: Remover este campo
        private int lastPayloadUncompressedLength; // TODO: Remover este campo

        public MainClient(PlayerInfo playerInfo, MarkersManager markersManager, Render render, PlayerActions playerActions, TargetsManager targetsManager, ClientNetworkManager clientNetworkManager)
        {
            this.playerInfo = playerInfo;
            this.markersManager = markersManager;
            this.render = render;
            this.playerActions = playerActions;
            this.targetsManager = targetsManager;
            this.clientNetworkManager = clientNetworkManager;
        }

        public async void CreateVehicle(uint modelHash, int primaryColor, int secondaryColor, int fuel, float x, float y, float z, float heading)
        {
            var model = new Model((VehicleHash)modelHash);
            var vehicle = await World.CreateVehicle(model, new Vector3(x, y, z), heading);
            API.SetVehicleColours(vehicle.Handle, primaryColor, secondaryColor);
            API.SetVehicleFuelLevel(vehicle.Handle, fuel);
        }

        public void DeleteVehicle(int vehicleHandle)
        {
            int vehicleHandleRef = vehicleHandle;
            API.DeleteVehicle(ref vehicleHandleRef);
        }

        public void GFSetPlayerMoney(int money)
        {
            render.UpdateMoney(this.playerInfo.Money, money);
            this.playerInfo.Money = money;
        }

        public async void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;

            if (clientInitializationStarted == false)
            {
                clientInitializationStarted = true;
                InitializeClient();
            }
        }

        private async void InitializeClient()
        {
            API.SetNuiFocus(false, false);

            API.DoScreenFadeOut(1000);
            while (API.IsScreenFadingOut())
            {
                await Delay(100);
            }
            API.ShutdownLoadingScreen();
            await Delay(1000);
            TriggerServerEvent("GF:Server:OnClientReady");
        }

        public void OpenNUIView(int nuiViewTypeInt, bool setFocus, string compressedJsonPayload, int uncompressedLength)
        {
            this.lastPayloadCompressed = compressedJsonPayload;
            this.lastPayloadUncompressedLength = uncompressedLength;
            var nuiViewType = (NUIViewType)nuiViewTypeInt;
            var payload = clientNetworkManager.Decompress(compressedJsonPayload, uncompressedLength);
            var nuiMessage = new
            {
                type = "OPEN_VIEW",
                viewName = nuiViewType.ToString(),
                payload = payload
            };
            var jsonEvent = JsonConvert.SerializeObject(nuiMessage);
            API.SendNuiMessage(jsonEvent);
            API.SetNuiFocus(setFocus, setFocus);
        }

        public void CloseNUIView(int nuiViewTypeInt, bool cancelFocus)
        {
            var nuiViewType = (NUIViewType)nuiViewTypeInt;

            var nuiMessage = new
            {
                type = "CLOSE_VIEW",
                viewName = nuiViewType.ToString()
            };
            var jsonEvent = JsonConvert.SerializeObject(nuiMessage);
            API.SendNuiMessage(jsonEvent);
            var focus = cancelFocus == false;
            API.SetNuiFocus(focus, focus);
        }

        public void OnPayloadReceive(int payloadTypeInt, string compressedPayload, int uncompressedLength)
        {
            var payload = clientNetworkManager.Decompress(compressedPayload, uncompressedLength);
            var subStringLenght = payload.Length < 80 ? payload.Length : payload.Length;
            Debug.WriteLine($"Payload[{compressedPayload.Length}:{payload.Length}]: { payload.Substring(0, subStringLenght)}..."); // TODO: Remove before release
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
                if (textInput == "/login") // HACK: Remove this command soon as possible
                {
                    OpenNUIView((int)NUIViewType.SELECT_ACCOUNT, true, lastPayloadCompressed, lastPayloadUncompressedLength);
                }
                var commandPacket = CommandParser.Parse(textInput);
                TriggerServerEvent("GF:Server:OnClientCommand", (int)commandPacket.CommandCode, commandPacket.HasArgs, commandPacket.Text);
            }
            else
                TriggerServerEvent("GF:Server:OnChatMessage", textInput);
        }

        public async void OnDie(int killerType, dynamic deathCoords)
        {
            API.CancelEvent();
            await Spawn.SpawnPlayer("S_M_Y_MARINE_01", 309.6f, -728.7297f, 29.3136f, 246.6142f); // TODO: Remove and call event when player dies, fix some bugs also.
        }

        public async void OnPlayerMapStart()
        {
            Debug.WriteLine("CLIENT EVENT: OnPlayerMapStart");
            API.CancelEvent();
            if (clientInitializationStarted == false)
            {
                clientInitializationStarted = true;
                InitializeClient();
            }
        }

        // Commands
        public async void OnInfo() // TODO: REMOVE THIS COMMAND
        {
            API.SendNuiMessage("{\"type\":\"CHARACTERS_SHOW\",\"enable\":true}");
            Screen.ShowNotification($"~b~ServerInfo~s~: Your are currently on the Tutorial Server By 4444, {Game.Player.Name}!");
        }

        public async void OnNuiEndpointCall(IDictionary<string, object> data, CallbackDelegate callbackResponse)
        {
            var responseType = data["type"].ToString();
            if (responseType == "RESPONSE_ACCOUNT_SELECTED")
            {
                var account = data["account"].ToString();
                TriggerServerEvent("GF:Server:ResponseAccountSelect", account);
            }
            callbackResponse("200");
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

        public void GFTeleportPlayerToPosition(Vector3 targetPosition, int transitionDurationInMs)
        {
            this.playerActions.TeleportPlayerToPosition(targetPosition, transitionDurationInMs);
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