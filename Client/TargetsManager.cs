using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public class TargetsManager : BaseScript
    {
        private TargetDto currentTarget;
        private DateTime? lastTargetReached;
        private bool playerReachedTarget;
        private readonly PlayerActions playerActions;

        public TargetsManager(PlayerActions playerActions)
        {
            this.Targets = new List<TargetDto>();
            this.playerActions = playerActions;
        }

        public List<TargetDto> Targets { get; set; }

        public async Task TargetsTickHandler()
        {
            var playerPosition = Game.PlayerPed.Position;

            if (currentTarget == null)
            {
                TargetDto bestTarget = null;
                float pX, pY, pZ;

                float closestDistance = float.MaxValue;
                foreach (var target in Targets)
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
                        this.currentTarget = bestTarget;

                        if (bestTarget.PeriodInMs <= 0)
                        {
                            if (currentTarget.OnEnterActionPayload.Length > 0)
                            {
                                OnTargetAction(currentTarget.ActionName, currentTarget.OnEnterActionPayload);
                            }
                        }
                        else if (bestTarget.PeriodInMs > 0 && this.lastTargetReached == null)
                        {
                            this.lastTargetReached = DateTime.Now;
                        }
                    }
                }
            }
            else
            {
                var radius = Math.Pow(currentTarget.Radius, 2);
                var distance = playerPosition.DistanceToSquared(new Vector3(this.currentTarget.X, this.currentTarget.Y, this.currentTarget.Z));
                if (distance > radius)
                {
                    this.lastTargetReached = null;

                    this.playerReachedTarget = false;
                    if (currentTarget.OnExitActionPayload.Length > 0)
                    {
                        OnTargetAction(currentTarget.ActionName, currentTarget.OnExitActionPayload);
                    }
                    this.currentTarget = null;
                }
                else
                {
                    // GFSendClientMessage((int)ChatColor.COLOR_ADD, $"Cara vai por aí nao::: distance: {distance}, radius: {radius} ");
                    if (this.lastTargetReached != null && (DateTime.Now - this.lastTargetReached).Value.TotalMilliseconds > currentTarget.PeriodInMs && playerReachedTarget == false)
                    {
                        playerReachedTarget = true;
                        if (currentTarget.OnEnterActionPayload.Length > 0)
                        {
                            OnTargetAction(currentTarget.ActionName, currentTarget.OnEnterActionPayload);
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
                    this.playerActions.GFPushNotification(payload, 2000);
                    API.PlaySound(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false, 0, true);
                    break;
            }
        }
    }
}