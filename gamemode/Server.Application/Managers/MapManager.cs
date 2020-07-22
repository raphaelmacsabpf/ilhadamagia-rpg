using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Application.Managers
{
    public class MapManager : BaseScript
    {
        private List<MarkerDto> staticMarkers;
        private List<ProximityTargetDto> staticProximityTargets;
        private List<InteractionTargetDto> staticInteractionTargets;
        private Dictionary<string, Action<GFPlayer, Player>> interactionTargetsCallbacks;
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
            this.staticProximityTargets = new List<ProximityTargetDto>();
            this.staticInteractionTargets = new List<InteractionTargetDto>();
            this.interactionTargetsCallbacks = new Dictionary<string, Action<GFPlayer, Player>>();
            this.houses = new List<GFHouse>();
            this.interiorPositions = new Dictionary<InteriorType, Vector3>();

            // Destroy all vehicles
            var currentVehicles = API.GetAllVehicles() as List<object>;
            foreach (var vehicle in currentVehicles)
            {
                // HACK: create function to delete all vehicles(Convert.ToInt32(vehicle));
            }

            BuildMarkers();
            BuildInteriors();
            BuildHouses();
            Console.WriteLine("[IM MapManager] Started MapManager");
        }

        public void BuildMarkers()
        {
            this.AddInterationMarkerWithNotification(307.8857f, -727.8989f, 29.3136f - 0.5f, MarkerColor.COLOR_ORANGE, "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.", (gfPlayer, player) =>
            {
                chatManager.SendClientMessage(gfPlayer, ChatColor.COLOR_ROSA, "aeEEEE Atingiu o target tio.");
            });

            this.AddInterationMarkerWithNotification(319.0022f, -713.1561f, 29.3136f - 0.5f, MarkerColor.COLOR_GREEN, "Bem vindo ao ~b~Ilha da Magia NEEEWS RPG~s~, aperte ~o~E~s~ para interagir.", (gfPlayer, player) =>
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
                this.AddInterationMarkerWithNotification(interiorPosition.X, interiorPosition.Y, interiorPosition.Z, MarkerColor.COLOR_BLUE, "Aperte ~o~E~s~ para sair", (gfPlayer, player) => HouseExit(gfPlayer));
            }
        }

        public void BuildHouses()
        {
            this.houses.Add(new GFHouse(1, 430.9583f, -1725.626f, 29.59998f, 432.3956f, -1736.519f, 28.58899f, 48.18897f));
            this.houses.Add(new GFHouse(2, -141.8901f, -1693.345f, 36.15454f, -141.1253f, -1702.009f, 30.10547f, 138.8976f));

            foreach (var house in this.houses)
            {
                this.AddInterationMarkerWithNotification(house.EntranceX, house.EntranceY, house.EntranceZ, MarkerColor.COLOR_BLUE, $"Casa de {house.Owner}, aperte ~o~E~s~ para entrar", (gfPlayer, player) => HouseEnter(gfPlayer, house));
            }
        }

        public void HouseEnter(GFPlayer gfPlayer, GFHouse house)
        {
            gfPlayer.CurrentHouse = house;
            this.playerActions.SetPlayerPos(gfPlayer, interiorPositions[house.Interior]);
        }

        private void HouseExit(GFPlayer gfPlayer)
        {
            var house = gfPlayer.CurrentHouse;
            if (house != null)
            {
                this.playerActions.SetPlayerPos(gfPlayer, new Vector3(house.EntranceX, house.EntranceY, house.EntranceZ));
                gfPlayer.CurrentHouse = null;
            }
        }

        public void AddInterationMarkerWithNotification(float x, float y, float z, MarkerColor color, string notification, Action<GFPlayer, Player> serverCallback)
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

        public void AddInteractionToServerCallbackTarget(float x, float y, float z, Action<GFPlayer, Player> serverCallback)
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

            if (interactionTargetsCallbacks.TryGetValue(callbackId, out Action<GFPlayer, Player> serverCallback))
            {
                var gfPlayer = this.playerInfo.GetGFPlayer(player);
                serverCallback(gfPlayer, player);
            }
        }

        public bool IsValidHouseId(int houseId)
        {
            return this.houses.Exists((house) =>
            {
                return house.GlobalId == houseId;
            });
        }

        public GFHouse GetGFHouseFromId(int id)
        {
            return this.houses.FirstOrDefault((house) =>
            {
                return house.GlobalId == id;
            });
        }
    }
}