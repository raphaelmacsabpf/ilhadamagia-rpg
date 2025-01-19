using CitizenFX.Core;
using Shared.CrossCutting;
using Shared.CrossCutting.Converters;
using Server.Application.Entities;
using Server.Application.Managers;
using Server.Application.Services;
using Server.Domain.Enums;
using Server.Domain.Services;
using System;
using Shared.CrossCutting.Enums;
using System.Dynamic;
using Server.Domain.Entities;
using VehicleEntity = Server.Domain.Entities.Vehicle;

namespace Server.Application.CommandLibraries
{
    public class AdminCommands : CommandLibrary
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly MapManager mapManager;
        private readonly MoneyService moneyService;
        private readonly PlayerService playerService;
        private readonly OrgService orgService;
        private readonly HouseService houseService;
        private readonly VehicleService vehicleService;

        public AdminCommands(ChatManager chatManager, 
                             PlayerInfo playerInfo, 
                             MapManager mapManager, 
                             MoneyService moneyService, 
                             PlayerService playerService, 
                             OrgService orgService,
                             HouseService houseService,
                             VehicleService vehicleService)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.mapManager = mapManager;
            this.moneyService = moneyService;
            this.playerService = playerService;
            this.orgService = orgService;
            this.houseService = houseService;
            this.vehicleService = vehicleService;
        }

        [Command("/ir")]
        public void Go(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /ir [playerid]"))
            {
                PlayerHandle playerHandle = commandValidator.GetTargetPlayerHandle();
                this.playerService.AdminGoToPlayer(commandValidator.SourcePlayerHandle, playerHandle);
            }
        }

        [Command("/trazer")]
        public void Bring(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /trazer [playerid]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                this.playerService.AdminBringPlayer(commandValidator.SourcePlayerHandle, targetPlayerHandle);
            }
        }

        [Command("/criarcasa")]
        public void CreateHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).IsValid("USE: /criarcasa"))
            {
                var playerHandle = commandValidator.SourcePlayerHandle;
                var position = playerHandle.Player.Character.Position;
                var heading = playerHandle.Player.Character.Heading;

                dynamic createHouseSession;
                if (!playerHandle.SessionVars.TryGetValue("create-house", out createHouseSession)) {
                    createHouseSession = new ExpandoObject();
                    createHouseSession.step = "start";
                }
                if (createHouseSession.step == "start")
                {
                    createHouseSession.step = "entrance";
                    playerHandle.SessionVars["create-house"] = createHouseSession;
                    chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_LIGHTBLUE, "Comando de multi passos, digite novamente em frente a entrada da casa");
                } 
                else if (createHouseSession.step == "entrance")
                {
                    createHouseSession.step = "vehicle";
                    createHouseSession.entranceX = position.X;
                    createHouseSession.entranceY = position.Y;
                    createHouseSession.entranceZ = position.Z;
                    playerHandle.SessionVars["create-house"] = createHouseSession;
                    chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_LIGHTBLUE, "Comando de multi passos, digite novamente no lugar do carro");
                }
                else if (createHouseSession.step == "vehicle")
                {
                    createHouseSession.vehicleX = position.X;
                    createHouseSession.vehicleY = position.Y;
                    createHouseSession.vehicleZ = position.Z;
                    createHouseSession.vehicleHeading = heading;

                    var house = new House();
                    house.Owner = playerHandle.Account.Username;
                    house.EntranceX = createHouseSession.entranceX;
                    house.EntranceY = createHouseSession.entranceY;
                    house.EntranceZ = createHouseSession.entranceZ;
                    house.VehiclePositionX = createHouseSession.vehicleX;
                    house.VehiclePositionY = createHouseSession.vehicleY;
                    house.VehiclePositionZ = createHouseSession.vehicleZ;
                    house.VehicleHeading = createHouseSession.vehicleHeading;
                    house.Interior = InteriorType.LOW_END_APARTMENT;
                    house.PropertyType = PropertyType.House;
                    house.SellState = PropertySellState.SOLD;

                    this.houseService.Create(house);
                    playerHandle.SessionVars.Remove("create-house");
                    chatManager.SendClientMessage(playerHandle, ChatColor.COLOR_LIGHTBLUE, "Casa criada com sucesso");
                }
            }
        }

        [Command("/logcoords")]
        public void Save(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).IsValid("USE: /logcoords"))
            {
                var playerHandle = commandValidator.SourcePlayerHandle;
                var position = playerHandle.Player.Character.Position;
                var heading = playerHandle.Player.Character.Heading;

                Console.WriteLine($"COORDS: {position.X}, {position.Y}, {position.Z}, { heading }");
            }
        }

        [Command("/setadmin")]
        public void SetAdmin(CommandValidator commandValidator)
        {
            if (commandValidator.WithTargetPlayer("playerid").WithVarBetween<int>(0, 3001, "level").IsValid("USE: /setadmin [playerid] [nivel(0-3001)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                int level = commandValidator.GetVar<int>("level");

                if (level > commandValidator.SourcePlayerHandle.Account.MaxAdminLevel)
                {
                    commandValidator.SendCommandError($"Você não pode colocar um nivel de admin maior do que seu nível máximo de admin");
                    return;
                }

                this.playerService.SetAsAdmin(commandValidator.SourcePlayerHandle, targetPlayerHandle, level);

                if (targetPlayerHandle.Account.MaxAdminLevel < level)
                {
                    this.playerService.SetMaxAdmin(commandValidator.SourcePlayerHandle, targetPlayerHandle, level);
                }
            }
        }

        [Command("/setmaxadmin")]
        public void SetMaxAdmin(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(3001).WithTargetPlayer("playerid").WithVarBetween<int>(0, 3001, "level").IsValid("USE: /setmaxadmin [playerid] [nivel(0-3001)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                int level = commandValidator.GetVar<int>("level");
                this.playerService.SetMaxAdmin(commandValidator.SourcePlayerHandle, targetPlayerHandle, level);
            }
        }

        [Command("/setsaude")]
        public void SetHealth(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").WithVarBetween<int>(0, 1000, "health").IsValid("USE: /setsaude [playerid] [health(0-1000)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                int value = commandValidator.GetVar<int>("health");

                this.playerService.SetPlayerHealth(commandValidator.SourcePlayerHandle, targetPlayerHandle, value);
            }
        }

        [Command("/setcolete")]
        public void SetArmour(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").WithVarBetween<int>(0, 100, "armour").IsValid("USE: /setcolete [playerid] [colete(0-100)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                int value = commandValidator.GetVar<int>("armour");

                this.playerService.SetPlayerArmour(commandValidator.SourcePlayerHandle, targetPlayerHandle, value);
            }
        }

        [Command("/setdimensao")]
        public void Test(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("target").WithVarBetween<int>(0, 999, "dimension").IsValid("USE: /setdimensao [playerid] [dimensao(0-999)]"))
            {
                var targetPlayer = commandValidator.GetTargetPlayerHandle();
                var dimension = commandValidator.GetVar<int>("dimension");
                this.playerService.SetPlayerDimension(targetPlayer, dimension);
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, "Teste executado com sucesso");
            }
        }

        [Command("/coords")]
        public void GoToCoords(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).IsValid("USE: /coords [x, y, z]")) // TODO: Terminar proteção do comando /coords
            {
                var vectorStr = commandValidator.CommandPacket.Text.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                var vectorPositions = vectorStr.Split(',');
                var position = new Vector3(
                    float.Parse(vectorPositions[0]),
                    float.Parse(vectorPositions[1]),
                    float.Parse(vectorPositions[2])
                );
                this.playerService.SetPlayerPos(commandValidator.SourcePlayerHandle, position);
            }
        }

        [Command("/setcasa")]
        public void SetHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(3001).WithTargetPlayer("playerid").WithVarBetween<int>(1, mapManager.HouseCount, "house-id").IsValid($"USE: /setcasa [playerid] [id(1-{mapManager.HouseCount})]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                int houseId = commandValidator.GetVar<int>("house-id");
                playerService.SetPlayerSelectedHouse(commandValidator.SourcePlayerHandle, targetPlayerHandle, houseId);
            }
        }

        [Command("/ircasa")]
        public void GoToHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithVarBetween<int>(1, mapManager.HouseCount, "house-id").IsValid($"USE: /ircasa [id(1-{mapManager.HouseCount})]"))
            {
                int houseId = commandValidator.GetVar<int>("house-id");
                this.playerService.TeleportPlayerToHouse(commandValidator.SourcePlayerHandle, houseId);
            }
        }

        [Command("/setskin")]
        public void SetPlayerPed(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarBetween<int>(0, PedModelsConverter.GetPedModelMaxId(), "ped-model-id")
                .IsValid($"USE: /setskin [playerid] [id(0-{PedModelsConverter.GetPedModelMaxId()}]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var pedId = commandValidator.GetVar<int>("ped-model-id");
                var pedModelHash = PedModelsConverter.GetHashStringById(pedId);
                if (pedModelHash != null)
                {
                    playerService.SetSkin(commandValidator.SourcePlayerHandle, targetPlayerHandle, pedModelHash);
                }
            }
        }

        [Command("/setorg")]
        public void SetOrg(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid").WithVarString("org-id")
                .IsValid($"USE: /setorg [playerid] [org-id]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var orgId = commandValidator.GetVar<string>("org-id");
                var gfOrg = orgService.GetOrgById(orgId);
                if (gfOrg == null)
                {
                    commandValidator.SendCommandError($"org-id {orgId} inválido", $"USE: /setorg [playerid] [org-id]");
                    return;
                }
                orgService.InvitePlayerToOrg(gfOrg, commandValidator.SourcePlayerHandle, targetPlayerHandle);
            }
        }

        [Command("/setlider")]
        public void SetAsLeader(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarString("org-id")
                .IsValid($"USE: /setlider [playerid] [org-id]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var orgId = commandValidator.GetVar<string>("org-id");
                var gfOrg = orgService.GetOrgById(orgId);
                if (gfOrg == null)
                {
                    commandValidator.SendCommandError($"org-id {orgId} inválido", $"USE: /setorg [playerid] [org-id]");
                    return;
                }
                orgService.SetOrgLeader(gfOrg, commandValidator.SourcePlayerHandle, targetPlayerHandle);
            }
        }

        [Command("/dararma")]
        public void GiveWeapon(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarWeapon("weapon")
                .WithVarBetween<int>(0, 9999, "ammo-count")
                .IsValid($"USE: /dararma [playerid] [weapon] [ammo-count(0-9999)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var weaponModelHash = commandValidator.GetVar<GameWeaponHash>("weapon");
                var ammoCount = commandValidator.GetVar<int>("ammo-count");

                var weaponName = WeaponConverter.GetWeaponName(weaponModelHash);

                commandValidator.SourcePlayerHandle.GiveWeaponToPlayer((uint)weaponModelHash, ammoCount, false, true);
                this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourcePlayerHandle.Account.Username} lhe concedeu uma {weaponName} com {ammoCount} de munição");
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" Você concedeu uma {weaponName} com {ammoCount} de munição para {targetPlayerHandle.Account.Username}");
            }
        }

        [Command("/dardinheiro")]
        public void GiveMoney(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1337).WithTargetPlayer("playerid")
                .WithVarBetween<long>(0, 1000000000, "money")
                .IsValid($"USE: /dardinheiro [playerid] [money(0-1000000000]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var money = commandValidator.GetVar<long>("money");

                var moneyTransaction = moneyService.AdminGiveMoney(commandValidator.SourcePlayerHandle.Account, targetPlayerHandle.Account, money);

                if (moneyTransaction.Status == MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS)
                {
                    this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_GRAD2, $"* {targetPlayerHandle.Account.Username} não pode receber esta quantia no momento.");
                    return;
                }

                this.playerInfo.SendUpdatedPlayerVars(targetPlayerHandle); // TODO: Melhorar essa atualização de variáveis (não é tão prioritário assim ainda)
                this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourcePlayerHandle.Account.Username} lhe deu ${money} de dinheiro");
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" Você deu ${money} de dinheiro para {targetPlayerHandle.Account.Username}");
            }
        }

        [Command("/kill")]
        public void Kill(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /kill [playerid]"))
            {
                var targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                targetPlayerHandle.KillPlayer();
                this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $"*Admin {commandValidator.SourcePlayerHandle.Account.Username} te matou");
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $"*Você matou {targetPlayerHandle.Account.Username}");
            }
        }

        [Command("/veh")]
        public void CreateVehicle(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarVehicle("vehicle")
                .IsValid($"USE: /veh [playerid] [vehicle]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var vehicleHash = commandValidator.GetVar<GameVehicleHash>("vehicle");
                var vehicleName = VehicleConverter.GetVehicleName(vehicleHash);

                targetPlayerHandle.CreatePlayerVehicle(vehicleHash);
                this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourcePlayerHandle.Account.Username} lhe concedeu um {vehicleName}");
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" Você concedeu um {vehicleName} para {targetPlayerHandle.Account.Username}");
            }
        }

        [Command("/darveiculocasa")]
        public void GiveHouseVehicle(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarVehicle("vehicle")
                .IsValid($"USE: /darveiculocasa [playerid] [vehicle]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var vehicleHash = commandValidator.GetVar<GameVehicleHash>("vehicle");
                var vehicleName = VehicleConverter.GetVehicleName(vehicleHash);

                VehicleEntity vehicle = new VehicleEntity();
                vehicle.Fuel = 64;
                vehicle.Owner = targetPlayerHandle.Account.Username;
                vehicle.PrimaryColor = 1;
                vehicle.SecondaryColor = 1;
                vehicle.EngineHealth = 100;
                vehicle.Hash = (uint)vehicleHash;

                this.vehicleService.Create(vehicle);

                this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourcePlayerHandle.Account.Username} lhe concedeu um {vehicleName} para sua casa");
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" Você concedeu um {vehicleName} para a casa de {targetPlayerHandle.Account.Username}");
            }
        }

        [Command("/settempo")]
        public void SetTOD(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithVarBetween<int>(0, 23, "hours").WithVarBetween<int>(0, 59, "minutes").WithVarBetween<int>(10, 60000, "ms-per-minute").IsValid($"USE: /settempo [horas(0-23)] [minutos(0-59)] [ms-por-minuto(10-60000)]"))
            {
                var hours = commandValidator.GetVar<int>("hours");
                var minutes = commandValidator.GetVar<int>("minutes");
                var msPerMinute = commandValidator.GetVar<int>("ms-per-minute");
                var timeSpan = new TimeSpan(hours, minutes, 0);
                this.mapManager.SetWorldClockSettings(timeSpan, msPerMinute);
                this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourcePlayerHandle.Account.Username} atualizou as configurações de hora para {timeSpan.ToString(@"hh\:mm")} e {msPerMinute} milisegundos por minuto");
            }
        }
    }
}