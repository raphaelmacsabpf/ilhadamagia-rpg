using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared.CrossCutting.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Application
{
    public class TargetsManager : BaseScript
    {
        private ProximityTargetDto currentProximityTarget;
        private DateTime? lastProximityTargetReached;
        private bool playerReachedProximityTarget;
        private readonly PlayerActions playerActions;

        public TargetsManager(PlayerActions playerActions)
        {
            this.ProximityTargets = new List<ProximityTargetDto>();
            this.InteractionTargets = new List<InteractionTargetDto>();
            this.playerActions = playerActions;
        }

        public List<ProximityTargetDto> ProximityTargets { get; set; }
        public List<InteractionTargetDto> InteractionTargets { get; set; }

        public void OnInteractionKeyPressed()
        {
            var playerPosition = Game.PlayerPed.Position;
            InteractionTargetDto bestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var target in InteractionTargets)
            {
                var distanceToClosest = playerPosition.DistanceToSquared(new Vector3(target.X, target.Y, target.Z));
                if (distanceToClosest < closestDistance)
                {
                    bestTarget = target;
                    closestDistance = distanceToClosest;
                }
            }

            if (bestTarget != null)
            {
                if (closestDistance < 2)
                {
                    OnTargetAction(bestTarget.ActionName, bestTarget.OnInteractionPayload);
                }
            }
        }

        public async Task TargetsTickHandler()
        {
            var playerPosition = Game.PlayerPed.Position;

            if (currentProximityTarget == null)
            {
                ProximityTargetDto bestTarget = null;

                float closestDistance = float.MaxValue;
                foreach (var target in ProximityTargets)
                {
                    var distanceToClosest = playerPosition.DistanceToSquared(new Vector3(target.X, target.Y, target.Z));
                    if (distanceToClosest < closestDistance)
                    {
                        bestTarget = target;
                        closestDistance = distanceToClosest;
                    }
                }

                if (bestTarget != null)
                {
                    var radius = Math.Pow(bestTarget.Radius, 2);
                    if (closestDistance < radius)
                    {
                        this.currentProximityTarget = bestTarget;

                        if (bestTarget.PeriodInMs <= 0)
                        {
                            if (currentProximityTarget.OnEnterActionPayload.Length > 0)
                            {
                                OnTargetAction(currentProximityTarget.ActionName, currentProximityTarget.OnEnterActionPayload);
                            }
                        }
                        else if (bestTarget.PeriodInMs > 0 && this.lastProximityTargetReached == null)
                        {
                            this.lastProximityTargetReached = DateTime.Now;
                        }
                    }
                }
            }
            else
            {
                var radius = Math.Pow(currentProximityTarget.Radius, 2);
                var distance = playerPosition.DistanceToSquared(new Vector3(this.currentProximityTarget.X, this.currentProximityTarget.Y, this.currentProximityTarget.Z));
                if (distance > radius)
                {
                    this.lastProximityTargetReached = null;

                    this.playerReachedProximityTarget = false;
                    if (currentProximityTarget.OnExitActionPayload.Length > 0)
                    {
                        OnTargetAction(currentProximityTarget.ActionName, currentProximityTarget.OnExitActionPayload);
                    }
                    this.currentProximityTarget = null;
                }
                else
                {
                    if (this.lastProximityTargetReached != null && (DateTime.Now - this.lastProximityTargetReached).Value.TotalMilliseconds > currentProximityTarget.PeriodInMs && playerReachedProximityTarget == false)
                    {
                        playerReachedProximityTarget = true;
                        if (currentProximityTarget.OnEnterActionPayload.Length > 0)
                        {
                            OnTargetAction(currentProximityTarget.ActionName, currentProximityTarget.OnEnterActionPayload);
                        }
                    }
                }
            }

            await Delay(100);
        }

        public async void OnTargetAction(string actionName, string payload)
        {
            //GFSendClientMessage((int)ChatColor.COLOR_ADD, $"actionName: {actionName}, payload:{payload}");
            switch (actionName)
            {
                case "INFO_TO_PLAYER":
                    string message = payload;
                    this.playerActions.PushNotification(message, 2000);
                    API.PlaySound(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false, 0, true);
                    break;

                case "SERVER_CALLBACK":
                    {
                        string serverCallback = payload;
                        TriggerServerEvent("GF:Server:OnPlayerTargetActionServerCallback", serverCallback);
                        break;
                    }
            }
        }
    }
}