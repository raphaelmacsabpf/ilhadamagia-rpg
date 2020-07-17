using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Dto;
using Server.Entities;
using System;
using System.Collections.Generic;

namespace Server
{
    public class MapManager : BaseScript
    {
        private List<MarkerDto> staticMarkers;
        private List<ServerVehicle> serverVehicles;
        private List<ProximityTargetDto> staticProximityTargets;
        private List<InteractionTargetDto> staticInteractionTargets;
        private Dictionary<string, Action> interactionTargetsCallbacks;

        private readonly PlayerInfo playerInfo;
        private readonly ChatManager chatManager;

        public MapManager(PlayerInfo playerInfo, ChatManager chatManager) // TODO: Verify constructor to prevent to start from fivem
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;

            this.staticMarkers = new List<MarkerDto>();
            this.serverVehicles = new List<ServerVehicle>();
            this.staticProximityTargets = new List<ProximityTargetDto>();
            this.staticInteractionTargets = new List<InteractionTargetDto>();
            this.interactionTargetsCallbacks = new Dictionary<string, Action>();

            var currentVehicles = API.GetAllVehicles() as List<object>;

            foreach (var vehicle in currentVehicles)
            {
                API.DeleteEntity(Convert.ToInt32(vehicle));
            }

            this.AddInterationMarkerWithNotification(307.8857f, -727.8989f, 29.3136f - 0.5f, "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.", () =>
            {
                chatManager.SendClientMessageToAll(GF.CrossCutting.ChatColor.COLOR_ROSA, "aeEEEE Atingiu o target tio.");
            });

            this.AddInterationMarkerWithNotification(319.0022f, -713.1561f, 29.3136f - 0.5f, "Bem vindo ao ~b~Ilha da Magia NEEEWS RPG~s~, aperte ~o~E~s~ para interagir.", () =>
            {
                chatManager.SendClientMessageToAll(GF.CrossCutting.ChatColor.COLOR_NEWS, "aeEEEE Atingiu o target NEWS tio.");
            });

            Console.WriteLine("[IM MapManager] Started MapManager");
        }

        public void AddInterationMarkerWithNotification(float x, float y, float z, string notification, Action serverCallback)
        {
            this.AddDefaultMarker(x, y, z);
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

        public void AddInteractionToServerCallbackTarget(float x, float y, float z, Action serverCallback)
        {
            Random random = new Random();
            var callbackId = $"{random.Next()}.{random.Next()}.{random.Next()}.{serverCallback.GetHashCode()}";
            interactionTargetsCallbacks.Add(callbackId, serverCallback);
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "SERVER_CALLBACK", callbackId));
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

            if (interactionTargetsCallbacks.TryGetValue(callbackId, out Action serverCallback))
            {
                serverCallback();
            }
        }
    }
}