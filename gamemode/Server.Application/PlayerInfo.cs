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
        private ConcurrentDictionary<Player, GFPlayer> playerToGFPlayerDictionary;
        private ConcurrentQueue<Tuple<PlayerVarsDto, GFPlayer>> playerVarsToUpdateQueue;
        private Thread updatePlayerVarsThread;
        private readonly NetworkManager networkManager;

        public PlayerInfo(NetworkManager networkManager)
        {
            this.playerToGFPlayerDictionary = new ConcurrentDictionary<Player, GFPlayer>();
            this.playerVarsToUpdateQueue = new ConcurrentQueue<Tuple<PlayerVarsDto, GFPlayer>>();

            this.updatePlayerVarsThread = new Thread(UpdatePlayerVarsThreadHandler);
            this.updatePlayerVarsThread.Priority = ThreadPriority.Lowest;
            this.updatePlayerVarsThread.IsBackground = true;
            this.updatePlayerVarsThread.Start();
            this.networkManager = networkManager;
        }

        public void LoadGFPlayer(GFPlayer gfPlayer)
        {
            playerToGFPlayerDictionary.TryAdd(gfPlayer.Player, gfPlayer);
        }

        public void UnloadGFPlayer(GFPlayer gfPlayer)
        {
            playerToGFPlayerDictionary.TryRemove(gfPlayer.Player, out _); // TODO: Melhorar implementação de dicionário de playerinfo
        }

        public void SendUpdatedPlayerVars(GFPlayer gfPlayer)
        {
            PlayerVarsDto playerVars = new PlayerVarsDto();
            playerVars.TryAdd("Money", gfPlayer.Account.Money.ToString());
            playerVars.TryAdd("Username", gfPlayer.Account.Username);
            var json = JsonConvert.SerializeObject(playerVars);
            this.networkManager.SendPayloadToPlayer(gfPlayer.Player, PayloadType.TO_PLAYER_VARS, json);
        }

        public GFPlayer GetGFPlayer(Player player)
        {
            return playerToGFPlayerDictionary[player];
        }

        public IEnumerable<GFPlayer> GetGFPlayerList()
        {
            return this.playerToGFPlayerDictionary.Values;
        }

        private void UpdatePlayerVarsThreadHandler()
        {
            while (true)
            {
                Tuple<PlayerVarsDto, GFPlayer> playerVarsTuple;
                while (playerVarsToUpdateQueue.TryDequeue(out playerVarsTuple))
                {
                    var json = JsonConvert.SerializeObject(playerVarsTuple.Item1);
                    this.networkManager.SendPayloadToPlayer(playerVarsTuple.Item2.Player, PayloadType.TO_PLAYER_VARS, json);
                    Thread.Sleep(10);
                }

                Thread.Sleep(1000);
            }
        }

        // TODO: Rever sistema de atualização de variáveis do jogador., atualização 21/07/2020 - Foi mexido, falta validar
        private void OnPlayerVarUpdate(GFPlayer gfPlayer, string variable, string value)
        {
            PlayerVarsDto playerVarsDto = new PlayerVarsDto();
            playerVarsDto.AddOrUpdate(variable, value, (key, oldValue) => value);

            Tuple<PlayerVarsDto, GFPlayer> playerVarsTuple = new Tuple<PlayerVarsDto, GFPlayer>(playerVarsDto, gfPlayer);
            playerVarsToUpdateQueue.Enqueue(playerVarsTuple);
        }
    }
}