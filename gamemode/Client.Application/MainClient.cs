using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using Newtonsoft.Json;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private List<BlipDto> mapBlips;
        private bool clientInitializationStarted;
        private bool deathCheckIsEnable;
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
                        this.targetsManager.ProximityTargets.AddRange(JsonConvert.DeserializeObject<List<ProximityTargetDto>>(payload));
                        return;
                    }
                case PayloadType.TO_STATIC_INTERACTION_TARGETS:
                    {
                        this.targetsManager.InteractionTargets = JsonConvert.DeserializeObject<List<InteractionTargetDto>>(payload);
                        return;
                    }
                case PayloadType.TO_MAP_BLIPS:
                    {
                        this.mapBlips = JsonConvert.DeserializeObject<List<BlipDto>>(payload);
                        foreach (var mapBlip in this.mapBlips)
                        {
                            var blipHandle = API.AddBlipForCoord(mapBlip.X, mapBlip.Y, mapBlip.Z);
                            API.SetBlipSprite(blipHandle, mapBlip.SpriteId);
                            API.SetBlipColour(blipHandle, mapBlip.Colour);
                            API.SetBlipAsShortRange(blipHandle, true);
                            API.BeginTextCommandSetBlipName("STRING");
                            API.AddTextComponentString(mapBlip.Category);
                            API.EndTextCommandSetBlipName(blipHandle);
                            API.SetBlipScale(blipHandle, mapBlip.Scale);
                        }
                        return;
                    }
            }
        }

        public void OnClientText(string textInput)
        {
            bool cancelEvent = true;
            if (textInput[0] == '/')
            {
                var splitted = textInput.Split(' ');
                if (splitted[0] == "/login") // HACK: Remove this command soon as possible
                {
                    var jjj = float.Parse(splitted[1]);
                    API.AddExplosion(309.6f, -728.7297f, 29.3136f, (int)ExplosionType.GrenadeL, jjj, true, false, 1f);
                    //OpenNUIView((int)NUIViewType.SELECT_ACCOUNT, true, lastPayloadCompressed, lastPayloadUncompressedLength);
                }
                else
                {
                    var commandPacket = new CommandPacket(textInput);
                    TriggerServerEvent("GF:Server:OnClientCommand", commandPacket.Command, commandPacket.HasArgs, commandPacket.Text);
                }
                if (cancelEvent)
                {
                    API.CancelEvent();
                }
            }
            else
                TriggerServerEvent("GF:Server:OnChatMessage", textInput);
        }

        public async void OnDie(int killerType, dynamic deathCoords)
        {
            // TODO: Criar sistema de tratamento de mortes.
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

        public async void CreatePlayerVehicle(uint vehicleHashUInt)
        {
            var vehicleHash = (VehicleHash)vehicleHashUInt;
            var model = new Model(vehicleHash);
            var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);
            Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);
        }

        // GF Functions
        public void GFSendClientMessage(int chatColor, string message)
        {
            this.playerActions.SendMessageToPlayerChat((ChatColor)chatColor, message);
        }

        public async void GFSpawnPlayer(string skinName, float x, float y, float z, float heading, bool fastSpawn)
        {
            this.deathCheckIsEnable = true;
            this.playerActions.SpawnPlayer(skinName, x, y, z, heading, fastSpawn);
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

        public void GFSetPedHealth(int value)
        {
            this.playerActions.SetPlayerHealth(value);
        }

        public void GFSetPedArmour(int value)
        {
            this.playerActions.SetPlayerArmour(value);
        }

        public async Task Wait1SecondTickHandler()
        {
            await Delay(1000);
            var missionRowJailDoorHashKey = (uint)API.GetHashKey("v_ilev_ph_cellgate");

            // Mission Row cell 1
            var policeDepartmentCell1 = API.GetClosestObjectOfType(462.381f, -993.651f, 24.9149f, 1.0f, missionRowJailDoorHashKey, false, false, false);
            API.SetEntityRotation(policeDepartmentCell1, 0, 0, -90, 0, true);
            API.FreezeEntityPosition(policeDepartmentCell1, true);

            // Mission Row cell 2
            var policeDepartmentCell2 = API.GetClosestObjectOfType(462.331f, -998.152f, 24.9149f, 1.0f, missionRowJailDoorHashKey, false, false, false);
            API.SetEntityRotation(policeDepartmentCell2, -180, 180, 90, 1, true);
            API.FreezeEntityPosition(policeDepartmentCell2, true);

            // Mission Row cell 3
            var policeDepartmentCell3 = API.GetClosestObjectOfType(462.704f, -1001.92f, 24.9149f, 1.0f, missionRowJailDoorHashKey, false, false, false);
            API.SetEntityRotation(policeDepartmentCell3, -180, 180, 90, 1, true);
            API.FreezeEntityPosition(policeDepartmentCell3, true);

            // TODO: 29/08/2020 Implementar sistema de abre e fecha das portas da DP Mission Row

            // Hide police blips
            API.SetPoliceRadarBlips(false);
        }

        public async void SwitchOutPlayer()
        {
            if (API.IsScreenFadedOut())
            {
                API.DoScreenFadeIn(300);
                while (API.IsScreenFadedIn())
                {
                    await Delay(16);
                }
            }
            API.SwitchOutPlayer(Game.PlayerPed.Handle, 0, 1);
            while (API.GetPlayerSwitchState() != 5)
            {
                await Delay(1);
            }
            TriggerServerEvent("GF:Server:TriggerStateEvent", "switched-out");
        }

        public async void SwitchInPlayer(float x, float y, float z)
        {
            Game.PlayerPed.Position = new Vector3(x, y, z);
            if (API.IsScreenFadedOut())
            {
                API.DoScreenFadeIn(300);
                while (API.IsScreenFadedIn())
                {
                    await Delay(16);
                }
            }

            API.SwitchInPlayer(Game.PlayerPed.Handle);
            while (API.GetPlayerSwitchState() != 12)
            {
                await Delay(1);
            }
            TriggerServerEvent("GF:Server:TriggerStateEvent", "switched-in");
        }

        public void SyncPlayerDateTime(string serverDateTimeAsString)
        {
            var server = DateTime.Parse(serverDateTimeAsString);
            API.NetworkOverrideClockTime(server.Hour, server.Minute, server.Second);
            API.SetMillisecondsPerGameMinute(60000);
        }

        public async Task PlayerStateTickHandler()
        {
            if (this.deathCheckIsEnable)
            {
                await Delay(50);
                if (API.IsEntityDead(Game.PlayerPed.Handle))
                {
                    this.deathCheckIsEnable = false;
                    API.SetTimeScale(0.4f);
                    var deathScreenEffects = new[] { "DeathFailOut", "DeathFailNeutralIn", "DeathFailMPDark", "DeathFailMPIn", "RaceTurbo" };
                    var random = new Random().Next(deathScreenEffects.Length - 1);
                    API.StartScreenEffect(deathScreenEffects[random], 8000, false);
                    API.StartScreenEffect("CamPushInTrevor", 8000, false);
                    await Delay(6000);
                    API.DoScreenFadeOut(300);
                    while (API.IsScreenFadingOut())
                    {
                        await Delay(16);
                    }
                    await Delay(1000);
                    API.StopAllScreenEffects();
                    TriggerServerEvent("GF:Server:TriggerStateEvent", "die");
                    await Delay(3000);
                }
            }
            else
            {
                await Delay(1000);
            }
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