using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Dto.MenuActions;
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
        private readonly ChatManager chatManager;
        private readonly StateManager stateManager;
        private readonly GameEntitiesManager gameEntitiesManager;

        public MainServer(PlayerInfo playerInfo, CommandManager commandManager, MapManager mapManager, ChatManager chatManager, StateManager stateManager, GameEntitiesManager gameEntitiesManager)
        {
            this.playerInfo = playerInfo;
            this.CommandManager = commandManager;
            this.MapManager = mapManager;
            this.chatManager = chatManager;
            this.stateManager = stateManager;
            this.gameEntitiesManager = gameEntitiesManager;
            foreach (var player in this.Players)
            {
                this.PrepareFSMForPlayer(player);
                var playerHandle = playerInfo.GetPlayerHandle(player);
                playerHandle.FSM.Fire(PlayerConnectionTrigger.GAMEMODE_LOAD);
            }

            API.RegisterCommand("gmx", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source <= 0)
                {
                    foreach (var targetPlayer in playerInfo.GetPlayerHandleList())
                    {
                        targetPlayer.FSM.Fire(PlayerConnectionTrigger.PLAYER_DROPPED);
                    }
                }
            }), true);

            mapManager.CreateOrgsSpawn();

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
            var playerHandle = playerInfo.GetPlayerHandle(player);
            playerHandle.FSM.Fire(PlayerConnectionTrigger.CLIENT_READY);
        }

        internal void OnPlayerDropped([FromSource] Player player, string reason)
        {
            var playerHandle = this.playerInfo.GetPlayerHandle(player);
            playerHandle.FSM.Fire(PlayerConnectionTrigger.PLAYER_DROPPED);
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
            var playerHandle = playerInfo.GetPlayerHandle(player);
            stateManager.SelectAccountForPlayer(playerHandle, accountName);
        }

        internal void OnPlayerTriggerStateEvent([FromSource] Player player, string eventTriggered)
        {
            var playerHandle = playerInfo.GetPlayerHandle(player);
            switch (eventTriggered)
            {
                case "die":
                    {
                        if (playerHandle.FSM.CanFire(PlayerConnectionTrigger.PLAYER_DIED))
                        {
                            playerHandle.FSM.Fire(PlayerConnectionTrigger.PLAYER_DIED);
                        }
                        return;
                    }
                case "switched-out":
                    {
                        playerHandle.FSM.Fire(PlayerConnectionTrigger.SWITCHED_OUT);
                        return;
                    }
                case "switched-in":
                    {
                        playerHandle.FSM.Fire(PlayerConnectionTrigger.SWITCHED_IN);
                        return;
                    }
            }
        }

        internal void OnPlayerMenuAction([FromSource] Player player, int menuActionInt, string compressedPayload)
        {
            var uncompressedPayload = LZ4Utils.Decompress(compressedPayload);
            var menuAction = (MenuAction)menuActionInt;
            var playerHandle = this.playerInfo.GetPlayerHandle(player);

            switch (menuAction)
            {
                case MenuAction.CALL_HOUSE_VEHICLE:
                    var dto = JsonConvert.DeserializeObject<CallHouseVehicleDto>(uncompressedPayload);
                    MapManager.PlayerHandleCallPropertyVehicle(playerHandle, dto.VehicleGuid);
                    break;
                case MenuAction.ORG_EQUIP:
                    playerHandle.OrgEquip();
                    break;
            }
        }

        private void PrepareFSMForPlayer(Player player)
        {
            var playerHandle = new PlayerHandle(player);
            var fsm = stateManager.CreatePlayerConnectionFSM(playerHandle);
            playerHandle.FSM = fsm;
            playerInfo.LoadPlayerHandle(playerHandle);
        }

        internal void OnChatMessage([FromSource] Player player, string message) // TODO: PROTEGER OnChatMessage
        {
            var wholeMessageCharsIsUppercase = message.CompareTo(message.ToUpper()) == 0;
            var playerHandle = playerInfo.GetPlayerHandle(player);
            if (wholeMessageCharsIsUppercase)
            {
                chatManager.PlayerScream(playerHandle, message);
            }
            else
            {
                var messageToChat = $"[ID: {player.Handle}] {playerHandle.Account.Username} diz: {message}";
                chatManager.PlayerChat(playerHandle, messageToChat);
            }
        }
        internal void OnClientCommand([FromSource] Player sourcePlayer, string command, bool hasArgs, string text)
        {
            var sourcePlayerHandle = playerInfo.GetPlayerHandle(sourcePlayer);
            try
            {
                CommandManager.ProcessCommandForPlayer(sourcePlayerHandle, command, hasArgs, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[IM CommandManager] Unhandled command exception: " + ex.Message); // TODO: Inserir informações do player
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_LIGHTRED, "Comando não reconhecido, use /ajuda para ver alguns comandos!");
                this.chatManager.SendClientMessage(sourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, "Peça ajuda também a um Administrador, use /relatorio."); // This '.' DOT at the end is the trick
            }
        }

    }
}