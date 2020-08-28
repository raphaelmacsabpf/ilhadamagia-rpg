using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Dto;
using Server.Application.Entities;
using Server.Database;
using Server.Domain.Enums;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Application.Managers
{
    public class MapManager : BaseScript
    {
        private readonly List<MarkerDto> staticMarkers;
        private readonly List<ProximityTargetDto> staticProximityTargets;
        private readonly List<InteractionTargetDto> staticInteractionTargets;
        private readonly Dictionary<string, Action<GFPlayer, Player>> interactionTargetsCallbacks;
        private readonly List<GFHouse> houses;
        private readonly Dictionary<InteriorType, Vector3> interiorPositions;
        private readonly List<BlipDto> blips;

        private readonly PlayerInfo playerInfo;
        private readonly ChatManager chatManager;
        private readonly PlayerActions playerActions;
        private readonly HouseRepository houseRepository;

        public MapManager(PlayerInfo playerInfo, ChatManager chatManager, PlayerActions playerActions, HouseRepository houseRepository, VehicleRepository vehicleRepository)
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.playerActions = playerActions;
            this.houseRepository = houseRepository;
            this.VehicleRepository = vehicleRepository;
            this.staticMarkers = new List<MarkerDto>();
            this.staticProximityTargets = new List<ProximityTargetDto>();
            this.staticInteractionTargets = new List<InteractionTargetDto>();
            this.interactionTargetsCallbacks = new Dictionary<string, Action<GFPlayer, Player>>();
            this.houses = new List<GFHouse>();
            this.interiorPositions = new Dictionary<InteriorType, Vector3>();
            this.blips = new List<BlipDto>();

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

        public int HouseCount => this.houses.Count;

        public IEnumerable<GFHouse> GetAllHousesFromOwner(string ownerUsername)
        {
            return this.houses.Where((house) => house.Entity.Owner == ownerUsername);
        }

        public VehicleRepository VehicleRepository { get; }

        public void GFPlayerCallPropertyVehicle(GFPlayer gfPlayer, string vehicleGuid)
        {
            var playerVehicles = VehicleRepository.GetAccountVehicles(gfPlayer.Account);
            var vehicle = playerVehicles.FirstOrDefault((_) => _.Guid == vehicleGuid);
            if (vehicle == null) return;
            var gfHouse = GetClosestHouseInRadius(gfPlayer, 3.0f);
            playerActions.CreateVehicle(gfPlayer, new Vector3(gfHouse.Entity.VehiclePositionX, gfHouse.Entity.VehiclePositionY, gfHouse.Entity.VehiclePositionZ), gfHouse.Entity.VehicleHeading, vehicle);
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

        public List<BlipDto> PopUpdateBlipsPayload()
        {
            return this.blips;
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

        public GFHouse GetGFHouseFromId(int id)
        {
            return this.houses.FirstOrDefault((house) => house.Entity.Id == id);
        }

        private void BuildMarkers()
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

        private void BuildInteriors()
        {
            this.interiorPositions.Add(InteriorType.LOW_END_APARTMENT, new Vector3(266.1758f, -1007.327f, -101.0198f));
            this.interiorPositions.Add(InteriorType.MEDIUM_END_APARTMENT, new Vector3(346.7209f, -1012.338f, -99.19995f));

            foreach (var interiorPosition in this.interiorPositions.Values)
            {
                this.AddInterationMarkerWithNotification(interiorPosition.X, interiorPosition.Y, interiorPosition.Z, MarkerColor.COLOR_BLUE, "Aperte ~o~E~s~ para sair", (gfPlayer, player) => HouseExit(gfPlayer));
            }
        }

        private void BuildHouses()
        {
            var houseEntities = houseRepository.GetAll();
            foreach (var houseEntity in houseEntities)
            {
                var gfHouse = new GFHouse { Entity = houseEntity };
                this.houses.Add(gfHouse);
            }

            foreach (var house in this.houses)
            {
                this.AddInterationMarkerWithNotification(house.Entity.EntranceX, house.Entity.EntranceY, house.Entity.EntranceZ, MarkerColor.COLOR_BLUE, $"Casa de {house.Entity.Owner}, aperte ~o~E~s~ para entrar", (gfPlayer, player) => HouseEnter(gfPlayer, house));
            }
        }

        private GFHouse GetClosestHouseInRadius(GFPlayer gfPlayer, float radius)
        {
            var playerPosition = gfPlayer.Player.Character.Position;
            GFHouse closestHouse = null;
            var closestDistance = float.MaxValue;

            foreach (var gfHouse in this.houses)
            {
                var houseEntity = gfHouse.Entity;
                var distanceToClosest = playerPosition.DistanceToSquared(new Vector3(houseEntity.EntranceX, houseEntity.EntranceY, houseEntity.EntranceZ));
                if (!(distanceToClosest < closestDistance)) continue;
                closestHouse = gfHouse;
                closestDistance = distanceToClosest;
            }

            if (closestHouse == null) return null;
            return closestDistance < radius ? closestHouse : null;
        }

        public Vector3 GetHouseInteriorPosition(GFHouse house)
        {
            return interiorPositions[house.Entity.Interior];
        }

        private void HouseEnter(GFPlayer gfPlayer, GFHouse house)
        {
            gfPlayer.CurrentHouseInside = house;
            this.playerActions.TeleportPlayerToPosition(gfPlayer, GetHouseInteriorPosition(house), 1000);
        }

        private void HouseExit(GFPlayer gfPlayer)
        {
            var house = gfPlayer.CurrentHouseInside;
            if (house != null)
            {
                this.playerActions.TeleportPlayerToPosition(gfPlayer, new Vector3(house.Entity.EntranceX, house.Entity.EntranceY, house.Entity.EntranceZ), 3000);
                gfPlayer.CurrentHouseInside = null;
            }
        }

        private void AddInterationMarkerWithNotification(float x, float y, float z, MarkerColor color, string notification, Action<GFPlayer, Player> serverCallback)
        {
            this.AddDefaultMarker(x, y, z, color);
            this.AddProximityNotificationTarget(x, y, z, notification);
            this.AddInteractionToServerCallbackTarget(x, y, z, serverCallback);
        }

        private void AddProximityNotificationTarget(float x, float y, float z, string message)
        {
            this.staticProximityTargets.Add(new ProximityTargetDto(x, y, z, 1.5f, 0, "INFO_TO_PLAYER", message, ""));
        }

        private void AddInteractionNotificationTarget(float x, float y, float z, string message)
        {
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "INFO_TO_PLAYER", message));
        }

        private void AddInteractionToServerCallbackTarget(float x, float y, float z, Action<GFPlayer, Player> serverCallback)
        {
            var random = new Random();
            var callbackId = $"{random.Next()}.{random.Next()}.{random.Next()}.{serverCallback.GetHashCode()}";
            interactionTargetsCallbacks.Add(callbackId, serverCallback);
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, "SERVER_CALLBACK", callbackId));
        }

        private void AddDefaultMarker(float x, float y, float z, MarkerColor color)
        {
            this.staticMarkers.Add(new MarkerDto(2, x, y, z, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.7f, 0.7f, 0.7f, color, false, true, 2, true, null, null, false));
        }

        private bool IsValidHouseId(int houseId)
        {
            return this.houses.Exists((house) => house.Entity.Id == houseId);
        }

        public void CreateOrgsSpawn(List<GFOrg> orgs)
        {
            foreach (var org in orgs)
            {
                if (org.Entity.Id > 0)
                {
                    this.AddInterationMarkerWithNotification(org.Entity.SpawnX, org.Entity.SpawnY, org.Entity.SpawnZ, MarkerColor.COLOR_YELLOW, $"{org.Entity.Name}, aperte ~o~E~s~ para interagir", ((gfPlayer, player) => OnPlayerInteractWithOrg(gfPlayer, org)));
                }
            }
        }

        public void CreateAmmunationsStore(List<GFAmmunation> ammunations)
        {
            foreach (var ammunation in ammunations)
            {
                this.blips.Add(new BlipDto("Loja de Armas", 110, 45, ammunation.ShopCounterPosition.X, ammunation.ShopCounterPosition.Y, ammunation.ShopCounterPosition.Z, 0.9f));
                this.AddInterationMarkerWithNotification(ammunation.ShopCounterPosition.X, ammunation.ShopCounterPosition.Y, ammunation.ShopCounterPosition.Z, MarkerColor.COLOR_GREEN, $"Ammunation {ammunation.Name}, aperte ~o~E~s~ para interagir", ((gfPlayer, player) => OnPlayerInteractWithAmmunation(gfPlayer, ammunation)));
            }
        }

        private void OnPlayerInteractWithOrg(GFPlayer gfPlayer, GFOrg gfOrg)
        {
            var orgDataDto = new OrgDataDto()
            {
                Name = gfOrg.Entity.Name,
                Leader = gfOrg.Entity.Leader,
                Members = new List<string>() // TODO: Load org members from repository in the right place
            };

            playerActions.OpenMenu(gfPlayer, MenuType.Org, orgDataDto);
        }

        private void OnPlayerInteractWithAmmunation(GFPlayer gfPlayer, GFAmmunation ammunation)
        {
            playerActions.OpenMenu(gfPlayer, MenuType.Ammunation, ammunation.Name);
        }

        public void CreateGasStations(List<GFGasStation> gasStations)
        {
            foreach (var gasStation in gasStations)
            {
                this.blips.Add(new BlipDto("Posto de Gasolina", 361, 5, gasStation.Position.X, gasStation.Position.Y, gasStation.Position.Z, 0.60f));
                this.AddInterationMarkerWithNotification(gasStation.Position.X, gasStation.Position.Y, gasStation.Position.Z, MarkerColor.COLOR_GREEN, $"Posto de gasolina {gasStation.Name}, aperte ~o~E~s~ para interagir", ((gfPlayer, player) => OnPlayerInteractWithGasStation(gfPlayer, gasStation)));
            }
        }

        private void OnPlayerInteractWithGasStation(GFPlayer gfPlayer, GFGasStation gasStation)
        {
            playerActions.OpenMenu(gfPlayer, MenuType.GasStation, gasStation.Name);
        }

        private void OnPlayerInteractWithATM(GFPlayer gfPlayer, GFATM atm)
        {
            playerActions.OpenMenu(gfPlayer, MenuType.ATM, "Caixa Eletrônico");
        }

        public void CreateATMs(List<GFATM> atmList)
        {
            foreach (var atm in atmList)
            {
                this.blips.Add(new BlipDto("Caixa Eletrônico", 276, 2, atm.Position.X, atm.Position.Y, atm.Position.Z, 1f));
                this.AddInterationMarkerWithNotification(atm.Position.X, atm.Position.Y, atm.Position.Z, MarkerColor.COLOR_GREEN, $"Caixa eletrônico, aperte ~o~E~s~ para interagir", ((gfPlayer, player) => OnPlayerInteractWithATM(gfPlayer, atm)));
            }
        }

        public void CreateClothingStores(List<GFClothingStore> clothingStoreList)
        {
            foreach (var clothingStore in clothingStoreList)
            {
                this.blips.Add(new BlipDto("Loja de Roupas", 73, 0, clothingStore.Position.X, clothingStore.Position.Y, clothingStore.Position.Z, 0.75f));
                this.AddInterationMarkerWithNotification(clothingStore.Position.X, clothingStore.Position.Y, clothingStore.Position.Z, MarkerColor.COLOR_GREEN, $"Loja de roupas, aperte ~o~E~s~ para interagir", ((gfPlayer, player) => OnPlayerInteractWithClothingStore(gfPlayer, clothingStore)));
            }
        }

        private void OnPlayerInteractWithClothingStore(GFPlayer gfPlayer, GFClothingStore clothingStore)
        {
            playerActions.OpenMenu(gfPlayer, MenuType.ClothingStore, "Loja de Roupas");
        }
    }
}