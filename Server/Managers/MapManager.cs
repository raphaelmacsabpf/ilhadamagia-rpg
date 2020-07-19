using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting;
using GF.CrossCutting.Dto;
using Server.Entities;
using Server.Enums;
using System;
using System.Collections.Generic;

namespace Server.Managers
{
    public class MapManager : BaseScript
    {
        private List<MarkerDto> staticMarkers;
        private List<ServerVehicle> serverVehicles;
        private List<ProximityTargetDto> staticProximityTargets;
        private List<InteractionTargetDto> staticInteractionTargets;
        private Dictionary<string, Action<GFPlayer>> interactionTargetsCallbacks;
        private List<GFHouse> houses;
        private Dictionary<InteriorType, Vector3> interiorPositions;

        private readonly PlayerInfo playerInfo;
        private readonly ChatManager chatManager;
        private readonly PlayerActions playerActions;

        public MapManager(PlayerInfo playerInfo, ChatManager chatManager, PlayerActions playerActions) // TODO: Verify constructor to prevent to start from fivem
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.playerActions = playerActions;

            this.staticMarkers = new List<MarkerDto>();
            this.serverVehicles = new List<ServerVehicle>();
            this.staticProximityTargets = new List<ProximityTargetDto>();
            this.staticInteractionTargets = new List<InteractionTargetDto>();
            this.interactionTargetsCallbacks = new Dictionary<string, Action<GFPlayer>>();
            this.houses = new List<GFHouse>();
            this.interiorPositions = new Dictionary<InteriorType, Vector3>();

            // Destroy all vehicles
            var currentVehicles = API.GetAllVehicles() as List<object>;
            foreach (var vehicle in currentVehicles)
            {
                API.DeleteEntity(Convert.ToInt32(vehicle));
            }

            BuildMarkers();
            BuildInteriors();
            BuildHouses();
            Console.WriteLine("[IM MapManager] Started MapManager");
        }

        public void BuildMarkers()
        {
            this.AddInterationMarkerWithNotification(307.8857f, -727.8989f, 29.3136f - 0.5f, MarkerColor.COLOR_ORANGE, "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.", (gfPlayer) =>
            {
                chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_ROSA, "aeEEEE Atingiu o target tio.");
            });

            this.AddInterationMarkerWithNotification(319.0022f, -713.1561f, 29.3136f - 0.5f, MarkerColor.COLOR_GREEN, "Bem vindo ao ~b~Ilha da Magia NEEEWS RPG~s~, aperte ~o~E~s~ para interagir.", (gfPlayer) =>
            {
                chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_NEWS, "aeEEEE Atingiu o target NEWS tio.");
            });
        }

        public void BuildInteriors()
        {
            this.interiorPositions.Add(InteriorType.LOW_END_APARTMENT, new Vector3(266.1758f, -1007.327f, -101.0198f));
            this.interiorPositions.Add(InteriorType.MEDIUM_END_APARTMENT, new Vector3(346.7209f, -1012.338f, -99.19995f));

            foreach (var interiorPosition in this.interiorPositions.Values)
            {
                this.AddInterationMarkerWithNotification(interiorPosition.X, interiorPosition.Y, interiorPosition.Z, MarkerColor.COLOR_BLUE, "Aperte ~o~E~s~ para sair", (gfPlayer) => HouseExit(gfPlayer));
            }
        }

        public void BuildHouses()
        {
            this.houses.Add(new GFHouse(new Vector3(430.9583f, -1725.626f, 29.59998f), new Vector4(432.3956f, -1736.519f, 28.58899f, 48.18897f)));
            this.houses.Add(new GFHouse(new Vector3(-141.8901f, -1693.345f, 36.15454f), new Vector4(-141.1253f, -1702.009f, 30.10547f, 138.8976f)));

            foreach (var house in this.houses)
            {
                this.AddInterationMarkerWithNotification(house.Entrance.X, house.Entrance.Y, house.Entrance.Z, MarkerColor.COLOR_BLUE, $"Casa de {house.Owner}, aperte ~o~E~s~ para entrar", (gfPlayer) => HouseEnter(gfPlayer, house));
            }
        }

        public void HouseEnter(GFPlayer gfPlayer, GFHouse house)
        {
            gfPlayer.CurrentHouse = house;
            this.playerActions.SetPlayerPos(gfPlayer.Player, interiorPositions[house.Interior]);
        }

        private void HouseExit(GFPlayer gfPlayer)
        {
            if (gfPlayer.CurrentHouse != null)
            {
                this.playerActions.SetPlayerPos(gfPlayer.Player, gfPlayer.CurrentHouse.Entrance);
                gfPlayer.CurrentHouse = null;
            }
        }

        public void AddInterationMarkerWithNotification(float x, float y, float z, MarkerColor color, string notification, Action<GFPlayer> serverCallback)
        {
            this.AddDefaultMarker(x, y, z, color);
            this.AddProximityNotificationTarget(x, y, z, notification);
            this.AddInteractionToServerCallbackTarget(x, y, z, serverCallback);
        }

        public void AddProximityNotificationTarget(float x, float y, float z, string message)
        {
            this.staticProximityTargets.Add(new ProximityTargetDto(x, y, z, 1.5f, 0, "INFO_TO_PLAYER", message, ""));
        }

        public void AddInteractionNotificationTarget(float x, float y, float z, string message)
        {
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "INFO_TO_PLAYER", message));
        }

        public void AddInteractionToServerCallbackTarget(float x, float y, float z, Action<GFPlayer> serverCallback)
        {
            Random random = new Random();
            var callbackId = $"{random.Next()}.{random.Next()}.{random.Next()}.{serverCallback.GetHashCode()}";
            interactionTargetsCallbacks.Add(callbackId, serverCallback);
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "SERVER_CALLBACK", callbackId));
        }

        public void AddDefaultMarker(float x, float y, float z, MarkerColor color)
        {
            this.staticMarkers.Add(new MarkerDto(2, x, y, z, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.7f, 0.7f, 0.7f, color, false, true, 2, true, null, null, false));
        }

        public List<MarkerDto> PopUpdatedStaticMarkersPayload()
        {
            return this.staticMarkers;
        }

        public List<ProximityTargetDto> PopUpdatedStaticProximityTargetsPayload()
        {
            return this.staticProximityTargets;
        }

        public List<InteractionTargetDto> PopUpdatedStaticInteractionTargetsPayload()
        {
            return this.staticInteractionTargets;
        }

        public void OnPlayerTargetActionServerCallback([FromSource] Player player, string callbackId)
        {
            switch (callbackId)
            {
                case "MAIN_SPAWN_INTERACTION":
                    {
                        // TODO: Create main spawn interaction
                        return;
                    }
            }

            if (interactionTargetsCallbacks.TryGetValue(callbackId, out Action<GFPlayer> serverCallback))
            {
                var gfPlayer = this.playerInfo.GetGFPlayer(player);
                serverCallback(gfPlayer);
            }
        }
    }
}