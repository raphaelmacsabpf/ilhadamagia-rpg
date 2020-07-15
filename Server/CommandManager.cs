using CitizenFX.Core;
using GF.CrossCutting;
using Server.Entities;
using System;

namespace Server
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
            CommandPacket commandPacket = new CommandPacket();
            commandPacket.CommandCode = (CommandCode)commandCode;
            commandPacket.HasArgs = hasArgs;
            commandPacket.Text = text;

            GFPlayer sourceGFPlayer = this.playerInfo.PlayerToGFPlayer(sourcePlayer);
            string[] args = new string[0];
            if (commandPacket.HasArgs)
            {
                args = commandPacket.Text.Split(' ');
            }
            switch (commandPacket.CommandCode)
            {
                case CommandCode.INFO:
                    {
                        sourcePlayer.TriggerEvent("client:Client:Info");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD1, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD2, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD3, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD4, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD5, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GRAD6, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GREY, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_GREEN, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_RED, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTRED, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_VERMELHO, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTBLUE, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_LIGHTGREEN, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_YELLOW, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_YELLOW2, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_WHITE, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE1, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE2, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE3, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE4, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_FADE5, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_PURPLE, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_DBLUE, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ALLDEPT, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_NEWS, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ROSA, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_SABRINA, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_HELPER, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_OOC, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_ALIANCA_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.OBJECTIVE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_PC_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_GREEN_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_JOB_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_HIT_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_BLUE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ADD, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_GROVE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.COLOR_ANG, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_VAGOS_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_BALLAS_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_AZTECAS_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_TRIAD_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_MS13_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_TATTAGLIA_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_CORLEONE_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_CYAN_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        this.chatManager.SendClientMessageToAll(ChatColor.TEAM_ROTAM_COLOR, "Estou testando esta cor legal | " + $"[ID: {sourcePlayer.Handle}] {sourcePlayer.Name} diz: Teste");
                        return;
                    }
                // TODO: Criar este comando corretamente
                case CommandCode.VEH:
                    {
                        sourcePlayer.TriggerEvent("client:Client:Veh");
                        return;
                    }
                case CommandCode.KILL:
                    {
                        // sourcePlayer.TriggerEvent("GF:Client:DeleteVehicle", this.mapManager.lastHandle); // TODO: Não esquecer deste exemplo aqui e continuar ele.
                        this.playerActions.KillPlayer(sourcePlayer); // TODO: Algum dia, proteger esse comando para só admin pegar (desenvolver contas antes)
                        return;
                    }
                case CommandCode.GO:
                    {
                        if (sourceGFPlayer.AdminLevel < 1)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD2, "Você não é um administrador!");
                            return;
                        }

                        var targetPlayerStr = args[1];
                        Player targetPlayer = GetPlayerByIdOrName(targetPlayerStr);
                        if (targetPlayer == null)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD1, $"   {targetPlayerStr} não é um player ativo.");
                            return;
                        }

                        var targetPosition = targetPlayer.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???

                        this.playerActions.SetPlayerPos(sourcePlayer, targetPosition);
                        this.chatManager.ProxDetectorColors(10.0f, targetPlayer, $"*Admin {sourcePlayer.Name} veio até {targetPlayer.Name}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                        this.chatManager.ProxDetectorColors(10.0f, sourcePlayer, $"*Admin {sourcePlayer.Name} foi até {targetPlayer.Name}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                        return;
                    }
                case CommandCode.BRING:
                    {
                        var targetPlayerStr = args[1];
                        Player targetPlayer = GetPlayerByIdOrName(targetPlayerStr);
                        if (targetPlayer == null)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD1, $"   {targetPlayerStr} não é um player ativo.");
                            return;
                        }

                        var sourcePosition = sourcePlayer.Character.Position + new Vector3(0f, 2f, -1f); // I don't know why this -1f???? WTF???

                        this.playerActions.SetPlayerPos(targetPlayer, sourcePosition);
                        this.chatManager.ProxDetectorColors(10.0f, sourcePlayer, $"*Admin {sourcePlayer.Name} trouxe {targetPlayer.Name}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                        this.chatManager.ProxDetectorColors(10.0f, targetPlayer, $"*Admin {sourcePlayer.Name} levou {targetPlayer.Name}", ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE, ChatColor.COLOR_PURPLE);
                        return;
                    }
                case CommandCode.SCREAM:
                    {
                        var messageToScream = commandPacket.Text.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
                        this.chatManager.PlayerScream(sourcePlayer, messageToScream);
                        return;
                    }
                case CommandCode.SAVE:
                    {
                        var position = sourcePlayer.Character.Position;
                        var heading = sourcePlayer.Character.Heading;
                        Console.WriteLine($"Saved {args[1]} X,Y,Z,H::::: SetPlayerPosHeading({position.X}f, {position.Y}f, {position.Z}f, {heading}f);");
                        return;
                    }
                case CommandCode.SET_ADMIN:
                    {
                        if (false && sourceGFPlayer.AdminLevel < 3001) // TODO: Remove TRUE from condition, before beta release
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD2, "Você não está autorizado a usar este comando!");
                            return;
                        }

                        var targetPlayerStr = args[1];
                        var targetPlayer = GetPlayerByIdOrName(args[1]);
                        if (targetPlayer == null)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD1, $"   {targetPlayerStr} não é um player ativo.");
                            return;
                        }

                        int level;
                        if (Int32.TryParse(args[2], out level) == false)
                        {
                            level = 0;
                        }
                        /*else if (level < 0 || level > 3001) // TODO: Retornar essa proteção ao comando
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD2, "USE: /setadmin [playerid] [nivel(0-3001)]");
                            return;
                        }*/

                        var targetGFPlayer = this.playerInfo.PlayerToGFPlayer(targetPlayer);
                        targetGFPlayer.AdminLevel = level;
                        this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $"  Você foi promovido a nivel {level} de admin, pelo admin {sourcePlayer.Name}");
                        this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_LIGHTBLUE, $" Você promoveu {targetPlayer.Name} para nivel {level} de admin.");
                        targetGFPlayer.Money = level; // TODO: Remover esse money daqui quando criar sistema de contas
                        return;
                    }

                case CommandCode.SET_ARMOUR:
                    {
                        if (sourceGFPlayer.AdminLevel < 1)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD2, "Você não está autorizado a usar este comando!");
                            return;
                        }

                        var targetPlayerStr = args[1];
                        var targetPlayer = GetPlayerByIdOrName(args[1]);
                        if (targetPlayer == null)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD1, $"   {targetPlayerStr} não é um player ativo.");
                            return;
                        }

                        int value;
                        if (args[2] == null || Int32.TryParse(args[2], out value) == false)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD2, "USE: /setcolete [playerid] [valor(0-100)]");
                            return;
                        }
                        else if (value < 0 || value > 100)
                        {
                            this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_GRAD2, "USE: /setcolete [playerid] [valor(0-100)]");
                            return;
                        }

                        this.playerActions.SetPlayerArmour(targetPlayer, value);
                        this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_LIGHTBLUE, $"Você deu {value} de colete para {targetPlayer.Name}");
                        this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $"O Admin {targetPlayer.Name} te deu {value} de colete");

                        this.playerActions.GiveWeaponToPlayer(sourcePlayer, WeaponHash.HeavySniperMk2, 200, false, true);
                        return;
                    }
                default:
                    {
                        this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_LIGHTRED, "Comando não reconhecido, use /ajuda para ver alguns comandos!");
                        this.chatManager.SendClientMessage(sourcePlayer, ChatColor.COLOR_LIGHTBLUE, "Peça ajuda também a um Administrador, use /relatorio");
                        return;
                    }
            }
        }

        private Player GetPlayerByIdOrName(string playerStr)
        {
            int parsedId;
            bool parseSucceed = Int32.TryParse(playerStr, out parsedId);
            var playerList = new PlayerList();
            if (parseSucceed)
            {
                foreach (Player player in playerList)
                {
                    if (Int32.Parse(player.Handle) == parsedId)
                    {
                        return player;
                    }
                }
            }
            else
            {
                foreach (Player player in playerList)
                {
                    var loweredPlayerName = player.Name.ToLower();
                    if (loweredPlayerName == playerStr || loweredPlayerName.StartsWith(playerStr))
                    {
                        return player;
                    }
                }
            }

            return null;
        }
    }
}