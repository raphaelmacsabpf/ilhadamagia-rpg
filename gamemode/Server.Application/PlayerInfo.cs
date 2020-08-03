using CitizenFX.Core;
using Newtonsoft.Json;
using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Managers;
using Server.Database;
using Server.Domain.Entities;
using Shared.CrossCutting;
using Shared.CrossCutting.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Application
{
    public class PlayerInfo
    {
        private ConcurrentDictionary<Player, GFPlayer> playerToGFPlayerDictionary;
        private ConcurrentQueue<Tuple<PlayerVarsDto, GFPlayer>> playerVarsToUpdateQueue;
        private Thread updatePlayerVarsThread;
        private readonly NetworkManager networkManager;
        private readonly AccountRepository accountRepository;

        public PlayerInfo(NetworkManager networkManager, AccountRepository accountRepository)
        {
            this.playerToGFPlayerDictionary = new ConcurrentDictionary<Player, GFPlayer>();
            this.playerVarsToUpdateQueue = new ConcurrentQueue<Tuple<PlayerVarsDto, GFPlayer>>();

            this.updatePlayerVarsThread = new Thread(UpdatePlayerVarsThreadHandler);
            this.updatePlayerVarsThread.Priority = ThreadPriority.Lowest;
            this.updatePlayerVarsThread.IsBackground = true;
            this.updatePlayerVarsThread.Start();
            this.networkManager = networkManager;
            this.accountRepository = accountRepository;
        }

        public void UnloadGFPlayer(GFPlayer gfPlayer)
        {
            playerToGFPlayerDictionary.TryRemove(gfPlayer.Player, out _); // TODO: Melhorar implementação de dicionário de playerinfo
        }

        public async Task<GFPlayer> PreparePlayerAccounts(Player player)
        {
            var gfPlayer = new GFPlayer(player);
            gfPlayer.ConnectionState = PlayerConnectionState.CONNECTED;
            if(playerToGFPlayerDictionary.TryAdd(player, gfPlayer) == false)
            {
                throw new Exception("eRRRO AQUI MEU FI, VAI LA E CORRIGE PFVOR");
            }

            var license = player.Identifiers["license"];
            var accounts = (await accountRepository.GetAccountListByLicense(license)).ToList();
            if (accounts.Count > 0)
            {
                gfPlayer.Account = new Account(); // TODO: Improve temporary account
                foreach (var account in accounts)
                {
                    gfPlayer.LicenseAccounts.Add(account);
                    Console.WriteLine($"Encontrada conta #{account.Id} para username: {account.Username}");
                }
                gfPlayer.ConnectionState = PlayerConnectionState.LOADING_ACCOUNT;
            }
            else
            {
                // HACK: Terminar de criar conta aqui, remover dados estáticos e criar sistema de criação de contas
                gfPlayer.ConnectionState = PlayerConnectionState.NEW_ACCOUNT;
                /*var username = "raphael_santos";
                var password = "123456";
                Account newAccount = new Account();
                newAccount.License = license;
                newAccount.Username = username;
                newAccount.Password = password;
                gfPlayer.Account = newAccount;
                int globalId = await accountRepository.Create(newAccount);
                Console.WriteLine($"Registrada a conta #{globalId} para o username: {username}");*/
            }
            return gfPlayer;
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

        public GFPlayer GetGFPlayer(Player player)
        {
            if (playerToGFPlayerDictionary.TryGetValue(player, out _) == false)
            {
                Console.WriteLine("antes de encontrar a conta do handle " + player.Handle);
                var gfPlayer = PreparePlayerAccounts(player).Result;
                Console.WriteLine($"encontrou a conta do handle: {player.Handle} com estado: {gfPlayer.ConnectionState}");
                return gfPlayer;

                // HACK: CREATE FALLBACK
            }

            return playerToGFPlayerDictionary[player];
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