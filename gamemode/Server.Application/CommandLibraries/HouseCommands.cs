using CitizenFX.Core;
using GF.CrossCutting.Dto;
using Server.Application.Managers;
using Server.Domain.Services;
using Shared.CrossCutting;
using System;
using System.Linq;

namespace Server.Application.CommandLibraries
{
    public class HouseCommands : CommandLibrary
    {
        private readonly ChatManager chatManager;
        private readonly PlayerActions playerActions;
        private readonly VehicleService vehicleService;

        public HouseCommands(ChatManager chatManager, PlayerActions playerActions, VehicleService vehicleService)
        {
            this.chatManager = chatManager;
            this.playerActions = playerActions;
            this.vehicleService = vehicleService;
        }

        [Command("/casa")]
        public void MyHouse(CommandValidator commandValidator)
        {
            var playerHouse = commandValidator.SourceGFPlayer.SelectedHouse;
            if (playerHouse == null)
            {
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD1, "Você não possui uma casa válida");
                return;
            }

            var distanceFromPlayerToHerHouse = commandValidator.SourceGFPlayer.Player.Character.Position.DistanceToSquared(new Vector3(playerHouse.Entity.EntranceX, playerHouse.Entity.EntranceY, playerHouse.Entity.EntranceZ));
            Console.WriteLine($"Distance is: {distanceFromPlayerToHerHouse}"); // TODO: Remover este LOG quando entender os problemas de sincronia
            if (distanceFromPlayerToHerHouse > Math.Pow(1.5f, 2))
            {
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD1, "Você está muito longe da sua casa");
                return;
            }
            var vehicleList = vehicleService.GetAccountVehicles(commandValidator.SourceGFPlayer.Account);
            var vehiclesAsDto = vehicleList.Select((vehicleEntity) =>
            {
                return new VehicleDto()
                {
                    Guid = vehicleEntity.Guid,
                    Hash = vehicleEntity.Hash,
                    PrimaryColor = vehicleEntity.PrimaryColor,
                    SecondaryColor = vehicleEntity.SecondaryColor,
                    Fuel = vehicleEntity.Fuel,
                    EngineHealth = vehicleEntity.EngineHealth
                };
            });

            this.playerActions.OpenMenu(commandValidator.SourceGFPlayer, MenuType.House, vehiclesAsDto);
        }
    }
}