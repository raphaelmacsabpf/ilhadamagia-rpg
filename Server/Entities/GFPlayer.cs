using CitizenFX.Core;
using GF.CrossCutting.Dto;
using System;

namespace Server.Entities
{
    public class GFPlayer
    {
        public event EventHandler<PlayerUpdateVarsEventArgs> OnPlayerVarsUpdate;

        private int money;
        private string username;

        public GFPlayer(Player player) // TODO: Rename GFPlayer to GMPlayer
        {
            this.PlayerId = Int32.Parse(player.Handle);
            this.Player = player;
            this.License = player.Identifiers["license"];
            this.Username = ""; // TODO: Set sanitized player.Name as GFPlayer.Username
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public Player Player { get; }
        public int PlayerId { get; }

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
        public GFHouse CurrentHouse { get; set; }

        public PlayerVarsDto PopUpdatedPlayerVarsPayload()
        {
            PlayerVarsDto playerVars = new PlayerVarsDto();
            playerVars.TryAdd("Money", this.Money.ToString());
            playerVars.TryAdd("Username", this.Username);
            return playerVars;
        }
    }
}