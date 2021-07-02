using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Managers;
using Shared.CrossCutting;
using System;
using System.Collections.Generic;

namespace Server.Application
{
    public class MainServer : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly PlayerActions playerActions;
        private readonly ChatManager chatManager;
        private readonly StateManager stateManager;
        private readonly GameEntitiesManager gameEntitiesManager;

        public MainServer(PlayerInfo playerInfo, CommandManager commandManager, MapManager mapManager, NetworkManager networkManager, PlayerActions playerActions, ChatManager chatManager, StateManager stateManager, GameEntitiesManager gameEntitiesManager)
        {
            this.playerInfo = playerInfo;
            this.CommandManager = commandManager;
            this.MapManager = mapManager;
            this.networkManager = networkManager;
            this.playerActions = playerActions;
            this.chatManager = chatManager;
            this.stateManager = stateManager;
            this.gameEntitiesManager = gameEntitiesManager;
            foreach (var player in this.Players)
            {
                this.PrepareFSMForPlayer(player);
                var gfPlayer = playerInfo.GetGFPlayer(player);
                gfPlayer.FSM.Fire(PlayerConnectionTrigger.GAMEMODE_LOAD);
            }

            API.RegisterCommand("gmx", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source <= 0)
                {
                    foreach (var targetPlayer in playerInfo.GetGFPlayerList())
                    {
                        targetPlayer.FSM.Fire(PlayerConnectionTrigger.PLAYER_DROPPED);
                    }
                }
            }), true);

            gameEntitiesManager.OnOrgsLoad += (sender, orgs) =>
            {
                mapManager.CreateOrgsSpawn(orgs);
            };

            gameEntitiesManager.OnAmmunationsLoad += (sender, ammunations) =>
            {
                mapManager.CreateAmmunationsStore(ammunations);
            };

            gameEntitiesManager.OnGasStationsLoad += (sender, gasStations) =>
            {
                mapManager.CreateGasStations(gasStations);
            };

            gameEntitiesManager.OnATMListLoad += (sender, atmList) =>
            {
                mapManager.CreateATMs(atmList);
            };

            gameEntitiesManager.OnClothingStoresLoad += (sender, clothingStoreList) =>
            {
                mapManager.CreateClothingStores(clothingStoreList);
            };

            gameEntitiesManager.OnHospitalsLoad += (sender, hospitalList) =>
            {
                mapManager.CreateHospitals(hospitalList);
            };

            gameEntitiesManager.OnPoliceDepartmentsLoad += (sender, policeDepartmentList) =>
            {
                mapManager.CreatePoliceDepartments(policeDepartmentList);
            };

            gameEntitiesManager.On247StoresLoad += (sender, store247List) =>
            {
                mapManager.Create247Stores(store247List);
            };

            gameEntitiesManager.InvokeInitialEvents();
            Console.WriteLine("[IM MainServer] Started MainServer");
        }

        internal CommandManager CommandManager { get; }

        internal MapManager MapManager { get; }

        internal async void OnClientReady([FromSource] Player player)
        {
            this.PrepareFSMForPlayer(player);
            var gfPlayer = playerInfo.GetGFPlayer(player);
            gfPlayer.FSM.Fire(PlayerConnectionTrigger.CLIENT_READY);
        }

        internal void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var gfPlayer = this.playerInfo.GetGFPlayer(player);
            gfPlayer.FSM.Fire(PlayerConnectionTrigger.PLAYER_DROPPED);
        }

        internal async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            // Mandatory Wait by FiveM
            await Delay(0);

            Console.WriteLine($"[Connecting] {playerName}, IP: {player.EndPoint}, Identifiers: {player.Identifiers["license"]}");
            for (int i = 0; i < 100; i++)
            {
                deferrals.update("Still checking:" + (i + 1));

                await Delay(1);
            }
            Console.WriteLine($"[Connected] ID:{player.Handle}, PlayerName: {playerName}, IP: {player.EndPoint}, License: {player.Identifiers["license"]}");
            deferrals.done();
        }

        internal void OnPlayerSelectAccount([FromSource] Player player, string accountName)
        {
            var gfPlayer = playerInfo.GetGFPlayer(player);
            stateManager.SelectAccountForPlayer(gfPlayer, accountName);
        }

        internal void OnPlayerTriggerStateEvent([FromSource] Player player, string eventTriggered)
        {
            var gfPlayer = playerInfo.GetGFPlayer(player);
            switch (eventTriggered)
            {
                case "die":
                    {
                        if (gfPlayer.FSM.CanFire(PlayerConnectionTrigger.PLAYER_DIED))
                        {
                            gfPlayer.FSM.Fire(PlayerConnectionTrigger.PLAYER_DIED);
                        }
                        return;
                    }
                case "switched-out":
                    {
                        gfPlayer.FSM.Fire(PlayerConnectionTrigger.SWITCHED_OUT);
                        return;
                    }
                case "switched-in":
                    {
                        gfPlayer.FSM.Fire(PlayerConnectionTrigger.SWITCHED_IN);
                        return;
                    }
            }
        }

        internal void OnPlayerMenuAction([FromSource] Player player, int menuActionInt, string compressedPayload)
        {
            var uncompressedPayload = networkManager.Decompress(compressedPayload);
            var menuAction = (MenuAction)menuActionInt;
            var gfPlayer = this.playerInfo.GetGFPlayer(player);

            switch (menuAction)
            {
                case MenuAction.CALL_HOUSE_VEHICLE:
                    var vehicleGuid = JsonConvert.DeserializeObject<string>(uncompressedPayload);
                    MapManager.GFPlayerCallPropertyVehicle(gfPlayer, vehicleGuid);
                    break;
            }
        }

        private void PrepareFSMForPlayer(Player player)
        {
            var gfPlayer = new GFPlayer(player);
            var fsm = stateManager.CreatePlayerConnectionFSM(gfPlayer);
            gfPlayer.FSM = fsm;
            playerInfo.LoadGFPlayer(gfPlayer);
        }

        internal void OnChatMessage([FromSource] Player player, string message) // TODO: PROTEGER OnChatMessage
        {
            var wholeMessageCharsIsUppercase = message.CompareTo(message.ToUpper()) == 0;
            var gfPlayer = playerInfo.GetGFPlayer(player);
            if (wholeMessageCharsIsUppercase)
            {
                chatManager.PlayerScream(gfPlayer, message);
            }
            else
            {
                var messageToChat = $"[ID: {player.Handle}] {gfPlayer.Account.Username} diz: {message}";
                chatManager.PlayerChat(gfPlayer, messageToChat);
            }
        }
        internal void OnClientCommand([FromSource] Player sourcePlayer, string command, bool hasArgs, string text)
        {
            var sourceGFPlayer = playerInfo.GetGFPlayer(sourcePlayer);
            try
            {
                CommandManager.ProcessCommandForPlayer(sourceGFPlayer, command, hasArgs, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[IM CommandManager] Unhandled command exception: " + ex.Message); // TODO: Inserir informações do player
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTRED, "Comando não reconhecido, use /ajuda para ver alguns comandos!");
                this.chatManager.SendClientMessage(sourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, "Peça ajuda também a um Administrador, use /relatorio."); // This '.' DOT at the end is the trick
            }
        }

    }
}