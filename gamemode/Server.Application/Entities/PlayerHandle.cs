using CitizenFX.Core;
using Server.Application.Enums;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Stateless;
using System.Collections.Generic;

namespace Server.Application.Entities
{
    public class PlayerHandle
    {
        private House selectedHouse;

        public PlayerHandle(Player player)
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

        public House SelectedHouse
        {
            get => selectedHouse;
            set
            {
                selectedHouse = value;
                if (value != null && this.Account != null)
                {
                    this.Account.SetSelectedHouse(value);
                    //HACK: Retirado refatoração DDD|this.Account.SelectedHouse = value.Entity.Id;
                }
            }
        }

        public SpawnType SpawnType { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Vector3 SwitchInPosition { get; set; }
        public bool IsFirstSpawn { get; set; }
        public House CurrentHouseInside { get; set; }
        public List<Account> LicenseAccounts { get; set; }
        public int OrgId { get; set; }
    }
}