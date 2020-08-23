using CitizenFX.Core;
using Server.Application.Enums;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Stateless;
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
            this.SpawnType = SpawnType.Unset;
            this.IsFirstSpawn = true;
        }

        public string License { get; }
        public StateMachine<PlayerConnectionState, PlayerConnectionTrigger> FSM { get; set; }
        public Account Account { get; set; }
        public Player Player { get; }

        public GFHouse SelectedHouse
        {
            get => selectedHouse;
            set
            {
                selectedHouse = value;
                if (value?.Entity != null && this.Account != null)
                {
                    this.Account.SelectedHouse = value.Entity.Id;
                }
            }
        }

        public SpawnType SpawnType { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Vector3 SwitchInPosition { get; set; }
        public bool IsFirstSpawn { get; set; }

        public GFHouse CurrentHouseInside { get; set; }
        public List<Account> LicenseAccounts { get; set; }
    }
}