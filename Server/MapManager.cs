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
        private List<TargetDto> staticTargets;

        public int lastHandle = 0;

        public MapManager(bool ignoreFivemInitialization) // TODO: Verify constructor to prevent to start from fivem
        {
            this.staticMarkers = new List<MarkerDto>();
            this.staticVehicles = new List<VehicleDto>();
            this.staticTargets = new List<TargetDto>();

            Console.WriteLine("[IM MapManager] Started MapManager");

            this.AddDefaultMarker(307.8857f, -727.8989f, 29.3136f - 0.5f);
            this.AddNotificationTarget(307.8857f, -727.8989f, 29.3136f - 0.5f, "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.");
        }

        public void AddNotificationTarget(float x, float y, float z, string message)
        {
            this.staticTargets.Add(new TargetDto(x, y, z, 1.5f, 0, "INFO_TO_PLAYER", "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.", ""));
        }

        public void AddDefaultMarker(float x, float y, float z)
        {
            this.staticMarkers.Add(new MarkerDto(2, x, y, z, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.7f, 0.7f, 0.7f, 255, 128, 0, 120, false, true, 2, true, null, null, false));
        }

        public List<MarkerDto> PopUpdatedStaticMarkersPayload()
        {
            return this.staticMarkers;
        }

        public List<TargetDto> PopUpdatedStaticTargetsPayload()
        {
            return this.staticTargets;
        }

        public void PlayerCreatedVehicle([FromSource] Player player, int vehicleHandle)
        {
            this.lastHandle = vehicleHandle;
            Console.WriteLine($"--------- CARRINHO CRIADO por {player.Name}, handle: {this.lastHandle}");
        }
    }
}