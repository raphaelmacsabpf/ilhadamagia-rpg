using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Managers;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Server.Application
{
    public class PlayerInfo
    {
        private ConcurrentDictionary<Player, PlayerHandle> playerToPlayerHandleDictionary;
        private ConcurrentQueue<Tuple<PlayerVarsDto, PlayerHandle>> playerVarsToUpdateQueue;
        private Thread updatePlayerVarsThread;

        public PlayerInfo()
        {
            this.playerToPlayerHandleDictionary = new ConcurrentDictionary<Player, PlayerHandle>();
            this.playerVarsToUpdateQueue = new ConcurrentQueue<Tuple<PlayerVarsDto, PlayerHandle>>();

            this.updatePlayerVarsThread = new Thread(UpdatePlayerVarsThreadHandler);
            this.updatePlayerVarsThread.Priority = ThreadPriority.Lowest;
            this.updatePlayerVarsThread.IsBackground = true;
            this.updatePlayerVarsThread.Start();
        }

        public void LoadPlayerHandle(PlayerHandle playerHandle)
        {
            playerToPlayerHandleDictionary.TryAdd(playerHandle.Player, playerHandle);
        }

        public void UnloadPlayerHandle(PlayerHandle playerHandle)
        {
            playerToPlayerHandleDictionary.TryRemove(playerHandle.Player, out _); // TODO: Melhorar implementação de dicionário de playerinfo
        }

        public void SendUpdatedPlayerVars(PlayerHandle playerHandle)
        {
            PlayerVarsDto playerVars = new PlayerVarsDto();
            playerVars.TryAdd("Money", playerHandle.Account.Money.ToString());
            playerVars.TryAdd("Username", playerHandle.Account.Username);
            var json = JsonConvert.SerializeObject(playerVars);
            playerHandle.SendPayloadToPlayer(PayloadType.TO_PLAYER_VARS, json);
        }

        public PlayerHandle GetPlayerHandle(Player player)
        {
            return playerToPlayerHandleDictionary[player];
        }

        public IEnumerable<PlayerHandle> GetPlayerHandleList()
        {
            return this.playerToPlayerHandleDictionary.Values;
        }

        private void UpdatePlayerVarsThreadHandler()
        {
            while (true)
            {
                Tuple<PlayerVarsDto, PlayerHandle> playerVarsTuple;
                while (playerVarsToUpdateQueue.TryDequeue(out playerVarsTuple))
                {
                    var json = JsonConvert.SerializeObject(playerVarsTuple.Item1);
                    playerVarsTuple.Item2.SendPayloadToPlayer(PayloadType.TO_PLAYER_VARS, json);
                    Thread.Sleep(10);
                }

                Thread.Sleep(1000);
            }
        }

        // TODO: Rever sistema de atualização de variáveis do jogador., atualização 21/07/2020 - Foi mexido, falta validar
        private void OnPlayerVarUpdate(PlayerHandle playerHandle, string variable, string value)
        {
            PlayerVarsDto playerVarsDto = new PlayerVarsDto();
            playerVarsDto.AddOrUpdate(variable, value, (key, oldValue) => value);

            Tuple<PlayerVarsDto, PlayerHandle> playerVarsTuple = new Tuple<PlayerVarsDto, PlayerHandle>(playerVarsDto, playerHandle);
            playerVarsToUpdateQueue.Enqueue(playerVarsTuple);
        }
    }
}