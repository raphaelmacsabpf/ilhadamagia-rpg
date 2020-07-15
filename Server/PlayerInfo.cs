using CitizenFX.Core;
using GF.CrossCutting;
using GF.CrossCutting.Dto;
using Newtonsoft.Json;
using Server.Entities;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Server
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

            this.updatePlayerVarsThread = new Thread(jjjj);
            this.updatePlayerVarsThread.Priority = ThreadPriority.Lowest;
            this.updatePlayerVarsThread.IsBackground = true;
            this.updatePlayerVarsThread.Start();
            this.networkManager = networkManager;
        }

        // TODO: Arrumar a porcaria do nome desse método
        private void jjjj()
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

        public GFPlayer PlayerToGFPlayer(Player player)
        {
            GFPlayer gfPlayer;
            if (playerToGFPlayerDictionary.TryGetValue(player, out _) == false)
            {
                gfPlayer = new GFPlayer(player);
                gfPlayer.OnPlayerVarsUpdate += GfPlayer_OnPlayerVarsUpdate;
                playerToGFPlayerDictionary.TryAdd(player, gfPlayer);
            }

            return playerToGFPlayerDictionary[player];
        }

        // TODO: Rever sistema de atualização de variáveis do jogador.
        private void GfPlayer_OnPlayerVarsUpdate(object sender, PlayerUpdateVarsEventArgs e)
        {
            var gfPlayer = sender as GFPlayer;

            Tuple<PlayerVarsDto, GFPlayer> playerVarsTuple;

            PlayerVarsDto playerVarsDto = new PlayerVarsDto();
            playerVarsDto.AddOrUpdate(e.PlayerVar, e.Value, (key, oldValue) => e.Value);
            playerVarsToUpdateQueue.Enqueue(new Tuple<PlayerVarsDto, GFPlayer>(playerVarsDto, gfPlayer));
        }
    }
}