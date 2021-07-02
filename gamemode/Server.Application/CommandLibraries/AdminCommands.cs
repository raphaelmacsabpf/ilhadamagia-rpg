using CitizenFX.Core;
using GF.CrossCutting;
using GF.CrossCutting.Converters;
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
        private readonly PlayerActions playerActions;
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly MapManager mapManager;
        private readonly GameEntitiesManager gameEntitiesManager;
        private readonly MoneyService moneyService;
        private readonly PlayerService playerService;
        private readonly OrgService orgService;

        public AdminCommands(PlayerActions playerActions, ChatManager chatManager, PlayerInfo playerInfo, MapManager mapManager, GameEntitiesManager gameEntitiesManager, MoneyService moneyService, PlayerService playerService, OrgService orgService)
        {
            this.playerActions = playerActions;
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.mapManager = mapManager;
            this.gameEntitiesManager = gameEntitiesManager;
            this.moneyService = moneyService;
            this.playerService = playerService;
            this.orgService = orgService;
        }

        [Command("/ir")]
        public void Go(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /ir [playerid]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                this.playerService.AdminGoToPlayer(commandValidator.SourceGFPlayer, targetGfPlayer);
            }
        }

        [Command("/trazer")]
        public void Bring(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /trazer [playerid]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                this.playerService.AdminBringPlayer(commandValidator.SourceGFPlayer, targetGfPlayer);
            }
        }

        [Command("/save")]
        public void Save(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithVarString("label").IsValid("USE: /save [rótulo]"))
            {
                var position = commandValidator.SourceGFPlayer.Player.Character.Position;
                var heading = commandValidator.SourceGFPlayer.Player.Character.Heading;
                Console.WriteLine($"Saved {commandValidator.GetVar<string>("label")} X,Y,Z,H::::: SetPlayerPosHeading({position.X}f, {position.Y}f, {position.Z}f, {heading}f);");
            }
        }

        [Command("/setadmin")]
        public void SetAdmin(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(3001).WithTargetPlayer("playerid").WithVarBetween<int>(0, 3001, "level").IsValid("USE: /setadmin [playerid] [nivel(0-3001)]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int level = commandValidator.GetVar<int>("level");
                this.playerService.SetAsAdmin(commandValidator.SourceGFPlayer, targetGfPlayer, level);
            }
        }

        [Command("/setsaude")]
        public void SetHealth(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").WithVarBetween<int>(0, 1000, "health").IsValid("USE: /setsaude [playerid] [health(0-1000)]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int value = commandValidator.GetVar<int>("health");

                this.playerService.SetPlayerHealth(commandValidator.SourceGFPlayer, targetGfPlayer, value);
            }
        }

        [Command("/setcolete")]
        public void SetArmour(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").WithVarBetween<int>(0, 100, "armour").IsValid("USE: /setcolete [playerid] [colete(0-100)]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int value = commandValidator.GetVar<int>("armour");

                this.playerService.SetPlayerArmour(commandValidator.SourceGFPlayer, targetGfPlayer, value);
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
                this.playerService.SetPlayerPos(commandValidator.SourceGFPlayer, position);
            }
        }

        [Command("/setcasa")]
        public void SetHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(3001).WithTargetPlayer("playerid").WithVarBetween<int>(1, mapManager.HouseCount, "house-id").IsValid($"USE: /setcasa [playerid] [id(1-{mapManager.HouseCount})]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int houseId = commandValidator.GetVar<int>("house-id");
                playerService.SetPlayerSelectedHouse(commandValidator.SourceGFPlayer, targetGfPlayer, houseId);
            }
        }

        [Command("/ircasa")]
        public void GoToHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithVarBetween<int>(1, mapManager.HouseCount, "house-id").IsValid($"USE: /ircasa [id(1-{mapManager.HouseCount})]"))
            {
                int houseId = commandValidator.GetVar<int>("house-id");
                this.playerService.TeleportPlayerToHouse(commandValidator.SourceGFPlayer, houseId);
            }
        }

        [Command("/setskin")]
        public void SetPlayerPed(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarBetween<int>(0, PedModelsConverter.GetPedModelMaxId(), "ped-model-id")
                .IsValid($"USE: /setskin [playerid] [id(0-{PedModelsConverter.GetPedModelMaxId()}]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var pedId = commandValidator.GetVar<int>("ped-model-id");
                var pedModelHash = PedModelsConverter.GetHashStringById(pedId);
                if (pedModelHash != null)
                {
                    playerService.SetSkin(commandValidator.SourceGFPlayer, targetGfPlayer, pedModelHash);
                }
            }
        }

        [Command("/setorg")]
        public void SetOrg(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarBetween<int>(0, PedModelsConverter.GetPedModelMaxId(), "org-id")
                .IsValid($"USE: /setorg [playerid] [org-id(0-{gameEntitiesManager.GetMaxOrgId()}]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var orgId = commandValidator.GetVar<int>("org-id");
                var gfOrg = gameEntitiesManager.GetGFOrgById(orgId);
                if (gfOrg == null)
                {
                    commandValidator.SendCommandError($"org-id {orgId} inválido", $"USE: /setorg [playerid] [id(0-{gameEntitiesManager.GetMaxOrgId()}]");
                    return;
                }
                orgService.InvitePlayerToOrg(gfOrg, commandValidator.SourceGFPlayer, targetGfPlayer);
            }
        }

        [Command("/setlider")]
        public void SetAsLeader(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarBetween<int>(0, PedModelsConverter.GetPedModelMaxId(), "org-id")
                .IsValid($"USE: /setlider [playerid] [org-id(0-{gameEntitiesManager.GetMaxOrgId()}]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var orgId = commandValidator.GetVar<int>("org-id");
                var gfOrg = gameEntitiesManager.GetGFOrgById(orgId);
                if (gfOrg == null)
                {
                    commandValidator.SendCommandError($"org-id {orgId} inválido", $"USE: /setorg [playerid] [id(0-{gameEntitiesManager.GetMaxOrgId()}]");
                    return;
                }
                orgService.SetOrgLeader(gfOrg, commandValidator.SourceGFPlayer, targetGfPlayer);
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
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var weaponId = commandValidator.GetVar<int>("weapon-id");
                var ammoCount = commandValidator.GetVar<int>("ammo-count");

                var weaponModelHash = WeaponConverter.GetWeaponHashById(weaponId);
                var weaponName = WeaponConverter.GetWeaponNameById(weaponId);

                this.playerActions.GiveWeaponToPlayer(commandValidator.SourceGFPlayer, (uint)weaponModelHash, ammoCount, false, true);
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourceGFPlayer.Account.Username} lhe concedeu uma {weaponName} com {ammoCount} de munição");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você concedeu uma {weaponName} com {ammoCount} de munição para {targetGfPlayer.Account.Username}");
            }
        }

        [Command("/dardinheiro")]
        public void GiveMoney(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1337).WithTargetPlayer("playerid")
                .WithVarBetween<long>(0, 1000000000, "money")
                .IsValid($"USE: /dardinheiro [playerid] [money(0-1000000000]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var money = commandValidator.GetVar<long>("money");

                var moneyTransaction = moneyService.AdminGiveMoney(commandValidator.SourceGFPlayer.Account, targetGfPlayer.Account, money);

                if (moneyTransaction.Status == MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS)
                {
                    this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD2, $"* {targetGfPlayer.Account.Username} não pode receber esta quantia no momento.");
                    return;
                }

                this.playerInfo.SendUpdatedPlayerVars(targetGfPlayer); // TODO: Melhorar essa atualização de variáveis (não é tão prioritário assim ainda)
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourceGFPlayer.Account.Username} lhe deu ${money} de dinheiro");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você deu ${money} de dinheiro para {targetGfPlayer.Account.Username}");
            }
        }

        [Command("/kill")]
        public void Kill(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /kill [playerid]"))
            {
                var targetGfPlayer = commandValidator.GetTargetGFPlayer();
                this.playerActions.KillPlayer(targetGfPlayer);
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $"*Admin {commandValidator.SourceGFPlayer.Account.Username} te matou");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $"*Você matou {targetGfPlayer.Account.Username}");
            }
        }

        [Command("/veh")]
        public void CreateVehicle(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithTargetPlayer("playerid")
                .WithVarBetween<int>(0, VehicleConverter.GetVehicleMaxId(), "vehicle-id")
                .IsValid($"USE: /veh [playerid] [vehicle-id(0-{VehicleConverter.GetVehicleMaxId()}]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var vehicleId = commandValidator.GetVar<int>("vehicle-id");
                var vehicleModelHash = VehicleConverter.GetVehicleHashById(vehicleId);
                var vehicleName = VehicleConverter.GetVehicleNameById(vehicleId);

                this.playerActions.CreatePlayerVehicle(targetGfPlayer, vehicleModelHash);
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" O admin {commandValidator.SourceGFPlayer.Account.Username} lhe concedeu um {vehicleName}");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você concedeu um {vehicleName} para {targetGfPlayer.Account.Username}");
            }
        }
    }
}