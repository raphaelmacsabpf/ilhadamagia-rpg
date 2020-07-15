using CitizenFX.Core;
using GF.CrossCutting.Dto;
using System;
using System.Collections.Generic;

namespace Server
{
    public class MapManager : BaseScript
    {
        private List<MarkerDto> staticMarkers;
        private List<VehicleDto> staticVehicles;
        private List<ProximityTargetDto> staticProximityTargets;
        private List<InteractionTargetDto> staticInteractionTargets;

        public int lastHandle = 0;
        private readonly PlayerInfo playerInfo;

        public MapManager(PlayerInfo playerInfo) // TODO: Verify constructor to prevent to start from fivem
        {
            this.staticMarkers = new List<MarkerDto>();
            this.staticVehicles = new List<VehicleDto>();
            this.staticProximityTargets = new List<ProximityTargetDto>();
            this.staticInteractionTargets = new List<InteractionTargetDto>();

            Console.WriteLine("[IM MapManager] Started MapManager");

            this.AddDefaultMarker(307.8857f, -727.8989f, 29.3136f - 0.5f);
            this.AddProximityNotificationTarget(307.8857f, -727.8989f, 29.3136f - 0.5f, "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.");
            this.AddInteractionToServerCallbackTarget(307.8857f, -727.8989f, 29.3136f - 0.5f, "MAIN_SPAWN_INTERACTION"); // TODO: Remover interação se necessário
            this.playerInfo = playerInfo;
        }

        public void AddProximityNotificationTarget(float x, float y, float z, string message)
        {
            this.staticProximityTargets.Add(new ProximityTargetDto(x, y, z, 1.5f, 0, "INFO_TO_PLAYER", message, ""));
        }

        public void AddInteractionNotificationTarget(float x, float y, float z, string message)
        {
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "INFO_TO_PLAYER", message));
        }

        public void AddInteractionToServerCallbackTarget(float x, float y, float z, string serverCallback)
        {
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "SERVER_CALLBACK", serverCallback));
        }

        public void AddDefaultMarker(float x, float y, float z)
        {
            this.staticMarkers.Add(new MarkerDto(2, x, y, z, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.7f, 0.7f, 0.7f, 255, 128, 0, 120, false, true, 2, true, null, null, false));
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

        public void PlayerCreatedVehicle([FromSource] Player player, int vehicleHandle)
        {
            this.lastHandle = vehicleHandle;
            Console.WriteLine($"--------- CARRINHO CRIADO por {player.Name}, handle: {this.lastHandle}");
        }

        public void OnPlayerTargetActionServerCallback([FromSource] Player player, string actionName, string payload)
        {
            switch (actionName)
            {
                case "MAIN_SPAWN_INTERACTION":
                    {
                        // TODO: Create main spawn interaction
                        break;
                    }
            }
        }
    }
}