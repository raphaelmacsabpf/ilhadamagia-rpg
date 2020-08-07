using CitizenFX.Core;
using Server.Application.Enums;
using Server.Domain.Entities;
using Stateless;
using System.Collections.Generic;

namespace Server.Application.Entities
{
    public class GFPlayer
    {
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
        public int SelectedHouseId { get; set; }
        public List<int> HouseIds { get; set; }
        public GFHouse CurrentHouse { get; set; }
        public List<Account> LicenseAccounts { get; set; }
    }
}