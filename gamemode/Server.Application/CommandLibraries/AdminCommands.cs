using CitizenFX.Core;
using GF.CrossCutting;
using GF.CrossCutting.Converters;
using Server.Application.Entities;
using Server.Application.Managers;
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
        private readonly StateManager stateManager;
        private readonly GameEntitiesManager gameEntitiesManager;
        private readonly MoneyService moneyService;

        public AdminCommands(PlayerActions playerActions, ChatManager chatManager, PlayerInfo playerInfo, MapManager mapManager, StateManager stateManager, GameEntitiesManager gameEntitiesManager, MoneyService moneyService)
        {
            this.playerActions = playerActions;
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.mapManager = mapManager;
            this.stateManager = stateManager;
            this.gameEntitiesManager = gameEntitiesManager;
            this.moneyService = moneyService;
        }

        [Command("/ir")]
        public void Go(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /ir [playerid]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var targetPosition = targetGfPlayer.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
                this.playerActions.SetPlayerPos(commandValidator.SourceGFPlayer, targetPosition);
                this.chatManager.ProxDetectorColorFixed(10.0f, targetGfPlayer, $"*Admin {commandValidator.SourceGFPlayer.Account.Username} veio até {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, new[] { commandValidator.SourceGFPlayer });
                this.chatManager.ProxDetectorColorFixed(10.0f, commandValidator.SourceGFPlayer, $"*Admin {commandValidator.SourceGFPlayer.Account.Username} foi até {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, new[] { targetGfPlayer });
            }
        }

        [Command("/trazer")]
        public void Bring(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").IsValid("USE: /trazer [playerid]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var sourcePosition = commandValidator.SourceGFPlayer.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
                this.playerActions.SetPlayerPos(targetGfPlayer, sourcePosition);
                this.chatManager.ProxDetectorColorFixed(10.0f, commandValidator.SourceGFPlayer, $"*Admin {commandValidator.SourceGFPlayer.Account.Username} trouxe {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, new[] { commandValidator.SourceGFPlayer });
                this.chatManager.ProxDetectorColorFixed(10.0f, targetGfPlayer, $"*Admin {commandValidator.SourceGFPlayer.Account.Username} levou {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, new[] { targetGfPlayer });
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

                targetGfPlayer.Account.SetAdminLevel(level);
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $"  Você foi promovido a nivel {level} de admin, pelo admin {commandValidator.SourceGFPlayer.Account.Username}");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você promoveu {targetGfPlayer.Account.Username} para nivel {level} de admin.");
            }
        }

        [Command("/setsaude")]
        public void SetHealth(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").WithVarBetween<int>(0, 1000, "health").IsValid("USE: /setsaude [playerid] [health(0-1000)]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int value = commandValidator.GetVar<int>("health");

                this.playerActions.SetPlayerHealth(targetGfPlayer, value);
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de saúde para {targetGfPlayer.Account.Username}");
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetGfPlayer.Account.Username} te deu {value} de saúde");
            }
        }

        [Command("/setcolete")]
        public void SetArmour(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).WithTargetPlayer("playerid").WithVarBetween<int>(0, 100, "armour").IsValid("USE: /setcolete [playerid] [colete(0-100)]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int value = commandValidator.GetVar<int>("armour");

                this.playerActions.SetPlayerArmour(targetGfPlayer, value);
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de colete para {targetGfPlayer.Account.Username}");
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetGfPlayer.Account.Username} te deu {value} de colete");
            }
        }

        [Command("/coords")]
        public void GoToCoords(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(1).IsValid("USE: /coords [x, y, z]")) // TODO: Terminar proteção do comando /coords
            {
                var vectorStr = commandValidator.CommandPacket.Text.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                var vectorPositions = vectorStr.Split(',');
                var targetVector = new Vector3(
                    float.Parse(vectorPositions[0]),
                    float.Parse(vectorPositions[1]),
                    float.Parse(vectorPositions[2])
                );
                this.playerActions.SetPlayerPos(commandValidator.SourceGFPlayer, targetVector);
            }
        }

        [Command("/setcasa")]
        public void SetHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(3001).WithTargetPlayer("playerid").WithVarBetween<int>(1, mapManager.HouseCount, "house-id").IsValid($"USE: /setcasa [playerid] [id(1-{mapManager.HouseCount})]"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                int houseId = commandValidator.GetVar<int>("house-id");
                targetGfPlayer.SelectedHouse = mapManager.GetGFHouseFromId(houseId);
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você setou a casa de {targetGfPlayer.Account.Username} para ID {houseId}.");
            }
        }

        [Command("/ircasa")]
        public void GoToHouse(CommandValidator commandValidator)
        {
            if (commandValidator.WithAdminLevel(4).WithVarBetween<int>(1, mapManager.HouseCount, "house-id").IsValid($"USE: /ircasa [id(1-{mapManager.HouseCount})]"))
            {
                int houseId = commandValidator.GetVar<int>("house-id");
                var gfHouse = mapManager.GetGFHouseFromId(houseId);
                this.playerActions.TeleportPlayerToPosition(commandValidator.SourceGFPlayer, new Vector3(gfHouse.Entity.EntranceX, gfHouse.Entity.EntranceY, gfHouse.Entity.EntranceZ), 500);
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
                    targetGfPlayer.Account.SetPedModel(pedModelHash);
                    stateManager.RespawnPlayerInCurrentPosition(targetGfPlayer);
                    this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" Sua skin foi alterada pelo admin {commandValidator.SourceGFPlayer.Account.Username}");
                    this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você alterou a skin de {targetGfPlayer.Account.Username}");
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
                //HACK: Retirado refatoração DDD|targetGfPlayer.Account.OrgId = orgId;
                //HACK: Retirado refatoração DDD|targetGfPlayer.Account.IsLeader = false;
                stateManager.RespawnPlayerInCurrentPosition(targetGfPlayer);
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" Sua organização foi alterada para {gfOrg.Entity.Name} pelo admin {commandValidator.SourceGFPlayer.Account.Username}");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você alterou a organização de {targetGfPlayer.Account.Username} para {gfOrg.Entity.Name}");
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
                //HACK: Retirado refatoração DDD|targetGfPlayer.Account.OrgId = orgId;
                //HACK: Retirado refatoração DDD|targetGfPlayer.Account.IsLeader = true;
                gfOrg.Entity.SetLeader(targetGfPlayer.Account);
                stateManager.RespawnPlayerInCurrentPosition(targetGfPlayer);
                this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você foi setado como lider da organização {gfOrg.Entity.Name} pelo admin {commandValidator.SourceGFPlayer.Account.Username}");
                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você setou {targetGfPlayer.Account.Username} como líder da organização {gfOrg.Entity.Name}");
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