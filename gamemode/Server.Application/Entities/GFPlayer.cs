using CitizenFX.Core;
using Server.Application.Enums;
using Server.Domain.Entities;
using Stateless;
using System;
using System.Collections.Generic;

namespace Server.Application.Entities
{
    public class GFPlayer
    {
        private GFHouse selectedHouse;

        public GFPlayer(Player player)
        {
            this.Player = player;
            this.License = player.Identifiers["license"];
            this.LicenseAccounts = new List<Account>();
        }

        public string License { get; }
        public StateMachine<PlayerConnectionState, PlayerConnectionTrigger> FSM { get; set; }
        public Account Account { get; set; }
        public Player Player { get; private set; }

        public GFHouse SelectedHouse
        {
            get
            {
                return selectedHouse;
            }
            set
            {
                selectedHouse = value;
                if (value != null && value.Entity != null && this.Account != null)
                {
                    this.Account.SelectedHouse = value.Entity.Id;
                }
            }
        }

        public GFHouse CurrentHouseInside { get; set; }
        public List<Account> LicenseAccounts { get; set; }
    }
}