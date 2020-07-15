using CitizenFX.Core;
using GF.CrossCutting;
using GF.CrossCutting.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server.Entities
{
    public class GFPlayer
    {
        public event EventHandler<PlayerUpdateVarsEventArgs> OnPlayerVarsUpdate;

        private int money;
        
        public GFPlayer(Player player)
        {
            this.PlayerId = Int32.Parse(player.Handle);
            this.Player = player;
            this.License = player.Identifiers["license"];
            this.Username = "";
        }

        public Player Player { get; }
        public int PlayerId { get; }

        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPlayerVarsUpdate?.Invoke(this, new PlayerUpdateVarsEventArgs("Username", value));
            }
        }
        public string Password { get; }
        public string License { get; }
        public int AdminLevel { get; set; }
        public int DonateRank { get; }
        public int Level { get; }
        public int Respect { get; }
        public int ConnectedTime { get; }

        public int Money
        {
            get { return money; }
            set
            {
                money = value;
                OnPlayerVarsUpdate?.Invoke(this, new PlayerUpdateVarsEventArgs("Money", value.ToString()));
            }
        }
        public int Bank { get; }

        public PlayerVarsDto PopUpdatedPlayerVarsPayload()
        {
            PlayerVarsDto playerVars = new PlayerVarsDto();
            playerVars.TryAdd("Money", this.Money.ToString());
            playerVars.TryAdd("Username", this.Username);
            return playerVars;
        }
    }
}
