using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared.CrossCutting.Dto;
using Server.Application.Entities;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Interfaces;
using Server.Domain.Services;
using Shared.CrossCutting;
using System;
using System.Collections.Generic;
using System.Linq;
using GF.CrossCutting.Enums;
using Server.Application.Services;
using GF.CrossCutting.Dto;

namespace Server.Application.Managers
{
    public class MapManager
    {
        private readonly List<MarkerDto> staticMarkers;
        private readonly List<ProximityTargetDto> staticProximityTargets;
        private readonly List<InteractionTargetDto> staticInteractionTargets;
        private readonly Dictionary<string, Action<PlayerHandle, Player>> interactionTargetsCallbacks;
        private readonly IEnumerable<House> houses;
        private readonly Dictionary<InteriorType, Vector3> interiorPositions;
        private readonly List<BlipDto> blips;

        private readonly PlayerInfo playerInfo;
        private readonly ChatManager chatManager;
        private readonly VehicleService vehicleService;
        private readonly OrgService orgService;
        private readonly HouseService houseService;
        private TimeSpan worldClock;
        private int millisecondsPerMinuteWorldClock;

        public MapManager(PlayerInfo playerInfo, ChatManager chatManager, HouseService houseService, VehicleService vehicleService, OrgService orgService)
        {
            this.playerInfo = playerInfo;
            this.chatManager = chatManager;
            this.vehicleService = vehicleService;
            this.orgService = orgService;
            this.staticMarkers = new List<MarkerDto>();
            this.staticProximityTargets = new List<ProximityTargetDto>();
            this.staticInteractionTargets = new List<InteractionTargetDto>();
            this.interactionTargetsCallbacks = new Dictionary<string, Action<PlayerHandle, Player>>();
            this.houseService = houseService;
            this.houses = this.houseService.GetAll();
            this.interiorPositions = new Dictionary<InteriorType, Vector3>();
            this.blips = new List<BlipDto>();
            this.worldClock = new TimeSpan(6, 0, 0);
            this.millisecondsPerMinuteWorldClock = 2000;

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

        public int HouseCount => this.houses.Count();

        public IEnumerable<House> GetAllHousesFromOwner(string ownerUsername)
        {
            return this.houses.Where((house) => house.Owner == ownerUsername);
        }

        public void PlayerHandleCallPropertyVehicle(PlayerHandle playerHandle, int vehicleId)
        {
            var playerVehicles = vehicleService.GetAccountVehicles(playerHandle.Account);
            var vehicle = playerVehicles.FirstOrDefault((_) => _.Id == vehicleId);
            if (vehicle == null) return;
            var gfHouse = GetClosestHouseInRadius(playerHandle, 3.0f);
            playerHandle.CreateVehicle(new Vector3(gfHouse.VehiclePositionX, gfHouse.VehiclePositionY, gfHouse.VehiclePositionZ), gfHouse.VehicleHeading, vehicle);
        }

        public void UpdateWorldClock()
        {
            var calculatedTime = this.worldClock.Add(TimeSpan.FromMilliseconds((60000 / this.millisecondsPerMinuteWorldClock) * 1000));
            this.worldClock = new TimeSpan(calculatedTime.Hours, calculatedTime.Minutes, calculatedTime.Seconds);
        }

        public TimeSpan GetWorldClock()
        {
            return new TimeSpan(this.worldClock.Hours, this.worldClock.Minutes, 0);
        }

        public void SetWorldClockSettings(TimeSpan time, int msPerMinute)
        {
            this.worldClock = time;
            this.millisecondsPerMinuteWorldClock = msPerMinute;
            foreach (PlayerHandle playerHandle in playerInfo.GetPlayerHandleList())
            {
                playerHandle.SyncPlayerDateTime(this.worldClock, this.millisecondsPerMinuteWorldClock);
            }
        }

        public int GetMillisecondsPerMinuteWorldClock()
        {
            return this.millisecondsPerMinuteWorldClock;
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

        public List<BlipDto> GetPlayerPrivateBlips(PlayerHandle playerHandle)
        {
            var playerHouses = houseService.GetAllFromAccount(playerHandle.Account);
            var privateBlips = new List<BlipDto>();
            foreach (var house in playerHouses)
            {
                privateBlips.Add(new BlipDto("Sua Casa", 40, 27, house.EntranceX, house.EntranceY, house.EntranceZ, 0.9f)); //TODO: Create blip colors and blip icons enums
            }

            return privateBlips;
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

            if (interactionTargetsCallbacks.TryGetValue(callbackId, out Action<PlayerHandle, Player> serverCallback))
            {
                var playerHandle = this.playerInfo.GetPlayerHandle(player);
                serverCallback(playerHandle, player);
            }
        }

        public House GetGFHouseFromId(int id)
        {
            return this.houses.FirstOrDefault((house) => house.Id == id);
        }

        private void BuildMarkers()
        {
            this.AddInterationMarkerWithNotification(307.8857f, -727.8989f, 29.3136f - 0.5f, 1.5f, MarkerColor.COLOR_ORANGE, "Bem vindo ao ~b~Ilha da Magia RPG~s~, aperte ~o~E~s~ para interagir.", (playerHandle, player) =>
            {
                chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_ROSA, "aeEEEE Atingiu o target tio.");
            });

            this.AddInterationMarkerWithNotification(319.0022f, -713.1561f, 29.3136f - 0.5f, 1.5f, MarkerColor.COLOR_GREEN, "Bem vindo ao ~b~Ilha da Magia NEEEWS RPG~s~, aperte ~o~E~s~ para interagir.", (playerHandle, player) =>
            {
                chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_NEWS, "aeEEEE Atingiu o target NEWS tio.");
            });
        }

        private void BuildInteriors()
        {
            this.interiorPositions.Add(InteriorType.LOW_END_APARTMENT, new Vector3(266.1758f, -1007.327f, -101.0198f));
            this.interiorPositions.Add(InteriorType.MEDIUM_END_APARTMENT, new Vector3(346.7209f, -1012.338f, -99.19995f));

            foreach (var interiorPosition in this.interiorPositions.Values)
            {
                this.AddInterationMarkerWithNotification(interiorPosition.X, interiorPosition.Y, interiorPosition.Z, 1.5f, MarkerColor.COLOR_BLUE, "Aperte ~o~E~s~ para sair", (playerHandle, player) => HouseExit(playerHandle));
            }
        }

        private void BuildHouses()
        {
            foreach (var house in this.houses)
            {
                this.AddInterationMarkerWithNotification(house.EntranceX, house.EntranceY, house.EntranceZ, 1.5f, MarkerColor.COLOR_BLUE, $"Casa de {house.Owner}, aperte ~o~E~s~ para entrar", (playerHandle, player) => HouseEnter(playerHandle, house));
            }
        }

        private House GetClosestHouseInRadius(PlayerHandle playerHandle, float radius)
        {
            var playerPosition = playerHandle.Player.Character.Position;
            House closestHouse = null;
            var closestDistance = float.MaxValue;

            foreach (var gfHouse in this.houses)
            {
                var distanceToClosest = playerPosition.DistanceToSquared(new Vector3(gfHouse.EntranceX, gfHouse.EntranceY, gfHouse.EntranceZ));
                if (!(distanceToClosest < closestDistance)) continue;
                closestHouse = gfHouse;
                closestDistance = distanceToClosest;
            }

            if (closestHouse == null) return null;
            return closestDistance < radius ? closestHouse : null;
        }

        public Vector3 GetHouseInteriorPosition(House house)
        {
            return interiorPositions[house.Interior];
        }

        private void HouseEnter(PlayerHandle playerHandle, House house)
        {
            playerHandle.CurrentHouseInside = house;
            playerHandle.TeleportPlayerToPosition(GetHouseInteriorPosition(house), 1000);
        }

        private void HouseExit(PlayerHandle playerHandle)
        {
            var house = playerHandle.CurrentHouseInside;
            if (house != null)
            {
                playerHandle.TeleportPlayerToPosition(new Vector3(house.EntranceX, house.EntranceY, house.EntranceZ), 3000);
                playerHandle.CurrentHouseInside = null;
            }
        }

        private void AddInterationMarkerWithNotification(float x, float y, float z, float radius, MarkerColor color, string notification, Action<PlayerHandle, Player> serverCallback)
        {
            this.AddDefaultMarker(x, y, z, color);
            this.AddProximityNotificationTarget(x, y, z, radius, notification);
            this.AddInteractionToServerCallbackTarget(x, y, z, radius, serverCallback);
        }

        private void AddProximityNotificationTarget(float x, float y, float z, float radius, string message)
        {
            this.staticProximityTargets.Add(new ProximityTargetDto(x, y, z, radius, 0, InteractionTargetAction.INFO_TO_PLAYER, message, ""));
        }

        private void AddInteractionNotificationTarget(float x, float y, float z, float radius, string message)
        {
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, radius, InteractionTargetAction.INFO_TO_PLAYER, message));
        }

        private void AddInteractionToServerCallbackTarget(float x, float y, float z, float radius, Action<PlayerHandle, Player> serverCallback)
        {
            var random = new Random();
            var callbackId = $"{random.Next()}.{random.Next()}.{random.Next()}.{serverCallback.GetHashCode()}";
            interactionTargetsCallbacks.Add(callbackId, serverCallback);
            this.staticInteractionTargets.Add(new InteractionTargetDto(x, y, z, radius, InteractionTargetAction.SERVER_CALLBACK, callbackId));
        }

        private void AddDefaultMarker(float x, float y, float z, MarkerColor color)
        {
            this.staticMarkers.Add(new MarkerDto(2, x, y, z, 0.0f, 0.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.7f, 0.7f, 0.7f, color, false, true, 2, true, null, null, false));
        }

        public void CreateOrgsSpawn()
        {
            var orgs = orgService.GetAllOrgs();
            foreach (var org in orgs)
            {
                this.AddInterationMarkerWithNotification(org.SpawnX, org.SpawnY, org.SpawnZ, 1.5f, MarkerColor.COLOR_YELLOW, $"{org.Name}, aperte ~o~E~s~ para interagir", ((playerHandle, player) => OnPlayerInteractWithOrg(playerHandle, org)));
            }
        }

        public void CreateAmmunationsStore(List<Ammunation> ammunations)
        {
            foreach (var ammunation in ammunations)
            {
                this.blips.Add(new BlipDto("Loja de Armas", 110, 45, ammunation.PositionX, ammunation.PositionY, ammunation.PositionZ, 0.9f));
                this.AddInterationMarkerWithNotification(ammunation.PositionX, ammunation.PositionY, ammunation.PositionZ, 1.5f, MarkerColor.COLOR_GREEN, $"Ammunation {ammunation.Name}, aperte ~o~E~s~ para interagir", ((playerHandle, player) => OnPlayerInteractWithAmmunation(playerHandle, ammunation)));
            }
        }

        private void OnPlayerInteractWithOrg(PlayerHandle playerHandle, Org org)
        {
            var orgMembers = orgService.GetOrgMembers(org).Select(o => new OrgMembershipDto() {
                Username = o.Username, 
                OrgId = o.OrgId, 
                Role = o.Role 
            }).ToList();

            var orgDataDto = new OrgDataDto()
            {
                Name = org.Name,
                Leader = "Leader_Name", // TODO: Load org leader name properly
                Members = orgMembers // TODO: Load org members from repository in the right place
            };

            playerHandle.OpenMenu(MenuType.Org, orgDataDto);
        }

        private void OnPlayerInteractWithAmmunation(PlayerHandle playerHandle, Ammunation ammunation)
        {
            playerHandle.OpenMenu(MenuType.Ammunation, ammunation.Name);
        }

        public void CreateGasStations(List<GasStation> gasStations)
        {
            foreach (var gasStation in gasStations)
            {
                this.blips.Add(new BlipDto("Posto de Gasolina", 361, 5, gasStation.PositionX, gasStation.PositionY, gasStation.PositionZ, 0.60f));
                this.AddInterationMarkerWithNotification(gasStation.PositionX, gasStation.PositionY, gasStation.PositionZ, 4.0f, MarkerColor.COLOR_GREEN, $"Posto de gasolina {gasStation.Name}, aperte ~o~E~s~ para interagir", ((playerHandle, player) => OnPlayerInteractWithGasStation(playerHandle, gasStation)));
            }
        }

        private void OnPlayerInteractWithGasStation(PlayerHandle playerHandle, GasStation gasStation)
        {
            playerHandle.OpenMenu(MenuType.GasStation, gasStation.Name);
        }

        private void OnPlayerInteractWithATM(PlayerHandle playerHandle, ATM atm)
        {
            playerHandle.OpenMenu(MenuType.ATM, "Caixa Eletrônico");
        }

        public void CreateATMs(List<ATM> atmList)
        {
            foreach (var atm in atmList)
            {
                this.blips.Add(new BlipDto("Caixa Eletrônico", 276, 2, atm.PositionX, atm.PositionY, atm.PositionZ, 0.5f));
                this.AddInterationMarkerWithNotification(atm.PositionX, atm.PositionY, atm.PositionZ, 1.5f, MarkerColor.COLOR_GREEN, $"Caixa eletrônico, aperte ~o~E~s~ para interagir", ((playerHandle, player) => OnPlayerInteractWithATM(playerHandle, atm)));
            }
        }

        public void CreateClothingStores(List<ClothingStore> clothingStoreList)
        {
            foreach (var clothingStore in clothingStoreList)
            {
                this.blips.Add(new BlipDto("Loja de Roupas", 73, 0, clothingStore.PositionX, clothingStore.PositionY, clothingStore.PositionZ, 0.75f));
                this.AddInterationMarkerWithNotification(clothingStore.PositionX, clothingStore.PositionY, clothingStore.PositionZ, 1.5f, MarkerColor.COLOR_GREEN, $"Loja de roupas, aperte ~o~E~s~ para interagir", ((playerHandle, player) => OnPlayerInteractWithClothingStore(playerHandle, clothingStore)));
            }
        }

        private void OnPlayerInteractWithClothingStore(PlayerHandle playerHandle, ClothingStore clothingStore)
        {
            playerHandle.OpenMenu(MenuType.ClothingStore, "Loja de Roupas");
        }

        private void OnPlayerInteractWithHospital(PlayerHandle playerHandle, Hospital hospital)
        {
            playerHandle.OpenMenu(MenuType.Hospital, "Hospital");
        }

        public void CreateHospitals(List<Hospital> hospitalList)
        {
            foreach (var hospital in hospitalList)
            {
                this.blips.Add(new BlipDto("Hospital", 428, 1, hospital.PositionX, hospital.PositionY, hospital.PositionZ, 0.75f));
                this.AddInterationMarkerWithNotification(hospital.PositionX, hospital.PositionY, hospital.PositionZ, 1.5f, MarkerColor.COLOR_GREEN, "Hospital, aperte ~o~E~s~ para interagir", ((playerHandle, Player) => OnPlayerInteractWithHospital(playerHandle, hospital)));
            }
        }

        public void CreatePoliceDepartments(List<PoliceDepartment> policeDepartmentList)
        {
            foreach (var policeDepartment in policeDepartmentList)
            {
                this.blips.Add(new BlipDto("Departamento de Polícia", 526, 38, policeDepartment.PositionX, policeDepartment.PositionY, policeDepartment.PositionZ, 0.75f));
            }
        }

        public void Create247Stores(List<The247Store> store247List)
        {
            foreach (var store247 in store247List)
            {
                this.blips.Add(new BlipDto("Loja 24/7", 59, 34, store247.PositionX, store247.PositionY, store247.PositionZ, 0.75f));
            }
        }
    }
}