using CitizenFX.Core;
using Server.Application.Entities;
using Server.Domain.Enums;
using Shared.CrossCutting;
using System;

namespace Server.Application.Managers
{
    public class CommandManager : BaseScript
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly PlayerActions playerActions;
        private readonly MapManager mapManager;

        public CommandManager(ChatManager chatManager, PlayerInfo playerInfo, NetworkManager networkManager, PlayerActions playerActions, MapManager mapManager)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.networkManager = networkManager;
            this.playerActions = playerActions;
            this.mapManager = mapManager;
            Console.WriteLine("[IM CommandManager] Started CommandManager");
        }

        internal void OnClientCommand([FromSource] Player sourcePlayer, int commandCode, bool hasArgs, string text)
        {
            var sourceGFPlayer = playerInfo.GetGFPlayer(sourcePlayer);
            try
            {
                ProcessCommandForPlayer(sourceGFPlayer, commandCode, hasArgs, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[IM CommandManager] Unhandled command exception: " + ex.Message); // TODO: Inserir informações do player
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTRED, "Comando não reconhecido, use /ajuda para ver alguns comandos!");
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, "Peça ajuda também a um Administrador, use /relatorio."); // This '.' DOT at the end is the trick
            }
        }

        private void ProcessCommandForPlayer(GFPlayer sourceGFPlayer, int commandCode, bool hasArgs, string text)
        {
            CommandPacket commandPacket = new CommandPacket();
            commandPacket.CommandCode = (CommandCode)commandCode;
            commandPacket.HasArgs = hasArgs;
            commandPacket.Text = text;

            string[] args = new string[0];
            if (commandPacket.HasArgs)
            {
                args = commandPacket.Text.Split(' ');
            }
            var commandValidator = new CommandValidator(this.playerInfo, this.chatManager, sourceGFPlayer, args);

            switch (commandPacket.CommandCode)
            {
                case CommandCode.INFO:
                    {
                        sourceGFPlayer.Player.TriggerEvent("client:Client:Info");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD1, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD2, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD3, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD4, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD5, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD6, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GREY, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GREEN, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_RED, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTRED, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_VERMELHO, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTBLUE, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTGREEN, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_YELLOW, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_YELLOW2, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_WHITE, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE1, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE2, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE3, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE4, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE5, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_PURPLE, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_DBLUE, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ALLDEPT, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_NEWS, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ROSA, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_SABRINA, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_HELPER, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_OOC, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_ALIANCA_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.OBJECTIVE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_PC_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_GREEN_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_JOB_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_HIT_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_BLUE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ADD, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_GROVE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ANG, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_VAGOS_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_BALLAS_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_AZTECAS_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_TRIAD_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_MS13_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_TATTAGLIA_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_CORLEONE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_CYAN_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_ROTAM_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourceGFPlayer.Player.Handle}] {sourceGFPlayer.Account.Username} diz: Teste");
                        return;
                    }
                // TODO: Criar este comando corretamente
                case CommandCode.VEH:
                    {
                        sourceGFPlayer.Player.TriggerEvent("client:Client:Veh");
                        return;
                    }
                case CommandCode.KILL:
                    {
                        // sourcePlayer.TriggerEvent("GF:Client:DeleteVehicle", this.mapManager.lastHandle); // TODO: Não esquecer deste exemplo aqui e continuar ele.
                        this.playerActions.KillPlayer(sourceGFPlayer); // TODO: Algum dia, proteger esse comando para só admin pegar (desenvolver contas antes)
                        return;
                    }
                case CommandCode.GO:
                    {
                        if (commandValidator.AdminLevel(1).TargetPlayer().IsValid())
                        {
                            GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                            var targetPosition = targetGfPlayer.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
                            this.playerActions.SetPlayerPos(sourceGFPlayer, targetPosition);
                            this.chatManager.ProxDetectorColors(10.0f, targetGfPlayer, $"*Admin {sourceGFPlayer.Account.Username} veio até {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                            this.chatManager.ProxDetectorColors(10.0f, sourceGFPlayer, $"*Admin {sourceGFPlayer.Account.Username} foi até {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                        }
                        return;
                    }
                case CommandCode.BRING:
                    {
                        if (commandValidator.AdminLevel(1).TargetPlayer().IsValid())
                        {
                            GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                            var sourcePosition = sourceGFPlayer.Player.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???
                            this.playerActions.SetPlayerPos(targetGfPlayer, sourcePosition);
                            this.chatManager.ProxDetectorColors(10.0f, sourceGFPlayer, $"*Admin {sourceGFPlayer.Account.Username} trouxe {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                            this.chatManager.ProxDetectorColors(10.0f, targetGfPlayer, $"*Admin {sourceGFPlayer.Account.Username} levou {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                        }
                        return;
                    }
                case CommandCode.SCREAM:
                    {
                        var messageToScream = commandPacket.Text.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                        this.chatManager.PlayerScream(sourceGFPlayer, messageToScream);
                        return;
                    }
                case CommandCode.SAVE:
                    {
                        var position = sourceGFPlayer.Player.Character.Position;
                        var heading = sourceGFPlayer.Player.Character.Heading;
                        Console.WriteLine($"Saved {args[1]} X,Y,Z,H::::: SetPlayerPosHeading({position.X}f, {position.Y}f, {position.Z}f, {heading}f);");
                        return;
                    }
                case CommandCode.SET_ADMIN:
                    {
                        if (commandValidator.AdminLevel(3001).TargetPlayer().IsValid())
                        {
                            GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                            int level;
                            if (Int32.TryParse(args[2], out level) == false)
                            {
                                level = 0;
                            }
                            else if (level < 0 || level > 3001)
                            {
                                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD2, "USE: /setadmin [playerid] [nivel(0-3001)]");
                                return;
                            }
                            targetGfPlayer.Account.AdminLevel = level;
                            this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $"  Você foi promovido a nivel {level} de admin, pelo admin {sourceGFPlayer.Account.Username}");
                            this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você promoveu {targetGfPlayer.Account.Username} para nivel {level} de admin.");
                        }
                        return;
                    }

                case CommandCode.SET_ARMOUR:
                    {
                        if (commandValidator.AdminLevel(1).TargetPlayer().IsValid())
                        {
                            GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                            int value;
                            if (args[2] == null || Int32.TryParse(args[2], out value) == false)
                            {
                                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD2, "USE: /setcolete [playerid] [valor(0-100)]");
                                return;
                            }
                            else if (value < 0 || value > 100)
                            {
                                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD2, "USE: /setcolete [playerid] [valor(0-100)]");
                                return;
                            }
                            this.playerActions.SetPlayerArmour(targetGfPlayer, value);
                            this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de colete para {targetGfPlayer.Account.Username}");
                            this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetGfPlayer.Account.Username} te deu {value} de colete");
                            this.playerActions.GiveWeaponToPlayer(sourceGFPlayer, WeaponHash.HeavySniperMk2, 200, false, true); // TODO: Remover isso quando sistema de armas estiver pronto
                        }
                        return;
                    }
                case CommandCode.GO_TO_COORDS:
                    {
                        if (commandValidator.AdminLevel(1).IsValid())
                        {
                            var vectorStr = commandPacket.Text.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                            var vectorPositions = vectorStr.Split(',');
                            var targetVector = new Vector3(
                                float.Parse(vectorPositions[0]),
                                float.Parse(vectorPositions[1]),
                                float.Parse(vectorPositions[2])
                            );
                            this.playerActions.SetPlayerPos(sourceGFPlayer, targetVector);
                        }
                        return;
                    }
                case CommandCode.PROP_MENU:
                    {
                        var playerHouse = this.mapManager.GetGFHouseFromId(sourceGFPlayer.SelectedHouseId);
                        if (playerHouse == null)
                        {
                            this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD1, "Você não possui uma casa válida");
                            return;
                        }

                        var distanceFromPlayerToHerHouse = sourceGFPlayer.Player.Character.Position.DistanceToSquared(new Vector3(playerHouse.Entity.EntranceX, playerHouse.Entity.EntranceY, playerHouse.Entity.EntranceZ));
                        Console.WriteLine($"Distance is: {distanceFromPlayerToHerHouse}"); // TODO: Remover este LOG quando entender os problemas de sincronia
                        if (distanceFromPlayerToHerHouse > Math.Pow(1.5f, 2))
                        {
                            this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD1, "Você está muito longe da sua casa");
                            return;
                        }

                        this.playerActions.OpenMenu(sourceGFPlayer, MenuType.House);
                        return;
                    }
                case CommandCode.SET_HOUSE:
                    {
                        if (commandValidator.AdminLevel(3001).IsValid())
                        {
                            GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                            var houseIdStr = args[2];
                            int houseId;
                            if (Int32.TryParse(houseIdStr, out houseId) == false || this.mapManager.IsValidHouseId(houseId) == false)
                            {
                                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD1, $"   {houseIdStr} não é um ID válido para uma casa.");
                                return;
                            }
                            targetGfPlayer.SelectedHouseId = houseId;
                            this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você setou a casa de {targetGfPlayer.Account.Username} para ID {houseId}.");
                        }
                        return;
                    }
                case CommandCode.GO_TO_HOUSE:
                    {
                        if (commandValidator.AdminLevel(4).IsValid())
                        {
                            var houseIdStr = args[1];
                            int houseId;
                            if (Int32.TryParse(houseIdStr, out houseId) == false || this.mapManager.IsValidHouseId(houseId) == false)
                            {
                                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_GRAD1, $"   {houseIdStr} não é um ID válido para uma casa.");
                                return;
                            }
                            var gfHouse = mapManager.GetGFHouseFromId(houseId);
                            this.playerActions.TeleportPlayerToPosition(sourceGFPlayer, new Vector3(gfHouse.Entity.EntranceX, gfHouse.Entity.EntranceY, gfHouse.Entity.EntranceZ), 500);
                        }
                        return;
                    }
                default:
                    {
                        this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTRED, "Comando não reconhecido, use /ajuda para ver alguns comandos!");
                        this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, "Peça ajuda também a um Administrador, use /relatorio");
                        return;
                    }
            }
        }
    }
}