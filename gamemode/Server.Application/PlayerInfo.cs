using CitizenFX.Core;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using Newtonsoft.Json;
using Server.Application.Managers;
using Server.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Server.Application
{
    public class PlayerInfo
    {
        private ConcurrentDictionary<Player, GFPlayer> playerToGFPlayerDictionary;
        private ConcurrentDictionary<GFPlayer, Player> gfPlayerToPlayerDictionary;
        private ConcurrentQueue<Tuple<PlayerVarsDto, GFPlayer>> playerVarsToUpdateQueue;
        private Thread updatePlayerVarsThread;
        private readonly NetworkManager networkManager;

        public PlayerInfo(NetworkManager networkManager)
        {
            this.playerToGFPlayerDictionary = new ConcurrentDictionary<Player, GFPlayer>();
            this.gfPlayerToPlayerDictionary = new ConcurrentDictionary<GFPlayer, Player>();
            this.playerVarsToUpdateQueue = new ConcurrentQueue<Tuple<PlayerVarsDto, GFPlayer>>();

            this.updatePlayerVarsThread = new Thread(UpdatePlayerVarsThreadHandler);
            this.updatePlayerVarsThread.Priority = ThreadPriority.Lowest;
            this.updatePlayerVarsThread.IsBackground = true;
            this.updatePlayerVarsThread.Start();
            this.networkManager = networkManager;
        }

        // TODO: Arrumar a porcaria do nome desse método
        private void UpdatePlayerVarsThreadHandler()
        {
            while (true)
            {
                Tuple<PlayerVarsDto, GFPlayer> playerVarsTuple;
                while (playerVarsToUpdateQueue.TryDequeue(out playerVarsTuple))
                {
                    var json = JsonConvert.SerializeObject(playerVarsTuple.Item1);
                    this.networkManager.SendPayloadToPlayer(GetPlayer(playerVarsTuple.Item2), PayloadType.TO_PLAYER_VARS, json);
                    Thread.Sleep(10);
                }

                Thread.Sleep(1000);
            }
        }

        public GFPlayer GetGFPlayer(Player player)
        {
            GFPlayer gfPlayer;
            if (playerToGFPlayerDictionary.TryGetValue(player, out _) == false)
            {
                gfPlayer = new GFPlayer(0, Int32.Parse(player.Handle)); // TODO: Load GFPlayer GlobalId
                playerToGFPlayerDictionary.TryAdd(player, gfPlayer);
                gfPlayerToPlayerDictionary.TryAdd(gfPlayer, player);
            }

            return playerToGFPlayerDictionary[player];
        }

        public Player GetPlayer(GFPlayer gfPlayer)
        {
            return gfPlayerToPlayerDictionary[gfPlayer];
        }

        public IEnumerable<GFPlayer> GetGFPlayerList()
        {
            return this.playerToGFPlayerDictionary.Values;
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