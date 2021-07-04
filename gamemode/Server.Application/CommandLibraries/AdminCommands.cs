using CitizenFX.Core;
using Shared.CrossCutting;
using Shared.CrossCutting.Converters;
using Server.Application.Entities;
using Server.Application.Managers;
using Server.Application.Services;
using Server.Domain.Enums;
using Server.Domain.Services;
using Shared.CrossCutting;
using System;

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

        public AdminCommands(ChatManager chatManager, 
                             PlayerInfo playerInfo, 
                             MapManager mapManager, 
                             MoneyService moneyService, 
                             PlayerService playerService, 
                             OrgService orgService)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.mapManager = mapManager;
            this.moneyService = moneyService;
            this.playerService = playerService;
            this.orgService = orgService;
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

        [Command("/save")]
        public void Save(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithVarString("label").IsValid("USE: /save [rótulo]"))
            {
                var position = commandValidator.SourcePlayerHandle.Player.Character.Position;
                var heading = commandValidator.SourcePlayerHandle.Player.Character.Heading;
                Console.WriteLine($"Saved {commandValidator.GetVar<string>("label")} X,Y,Z,H::::: SetPlayerPosHeading({position.X}f, {position.Y}f, {position.Z}f, {heading}f);");
            }
        }

        [Command("/setadmin")]
        public void SetAdmin(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(3001).WithTargetPlayer("playerid").WithVarBetween<int>(0, 3001, "level").IsValid("USE: /setadmin [playerid] [nivel(0-3001)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                int level = commandValidator.GetVar<int>("level");
                this.playerService.SetAsAdmin(commandValidator.SourcePlayerHandle, targetPlayerHandle, level);
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
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarBetween<int>(0, PedModelsConverter.GetPedModelMaxId(), "org-id")
                .IsValid($"USE: /setorg [playerid] [org-id]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var orgId = commandValidator.GetVar<int>("org-id");
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
                .WithVarBetween<int>(0, PedModelsConverter.GetPedModelMaxId(), "org-id")
                .IsValid($"USE: /setlider [playerid] [org-id]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var orgId = commandValidator.GetVar<int>("org-id");
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
                .WithVarBetween<int>(0, WeaponConverter.GetWeaponMaxId(), "weapon-id")
                .WithVarBetween<int>(0, 250, "ammo-count")
                .IsValid($"USE: /dararma [playerid] [weapon-id(0-{WeaponConverter.GetWeaponMaxId()}] [ammo-count(0-250)]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var weaponId = commandValidator.GetVar<int>("weapon-id");
                var ammoCount = commandValidator.GetVar<int>("ammo-count");

                var weaponModelHash = WeaponConverter.GetWeaponHashById(weaponId);
                var weaponName = WeaponConverter.GetWeaponNameById(weaponId);

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
                .WithVarBetween<int>(0, VehicleConverter.GetVehicleMaxId(), "vehicle-id")
                .IsValid($"USE: /veh [playerid] [vehicle-id(0-{VehicleConverter.GetVehicleMaxId()}]"))
            {
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var vehicleId = commandValidator.GetVar<int>("vehicle-id");
                var vehicleModelHash = VehicleConverter.GetVehicleHashById(vehicleId);
                var vehicleName = VehicleConverter.GetVehicleNameById(vehicleId);

                targetPlayerHandle.CreatePlayerVehicle(vehicleModelHash);
                this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourcePlayerHandle.Account.Username} lhe concedeu um {vehicleName}");
                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" Você concedeu um {vehicleName} para {targetPlayerHandle.Account.Username}");
            }
        }
    }
}