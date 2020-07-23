using CitizenFX.Core;
using Server.Application.Enums;
using Server.Domain.Entities;

namespace Server.Application.Entities
{
    public class GFPlayer
    {
        public GFPlayer(Player player)
        {
            this.Player = player;
        }

        public PlayerConnectionState ConnectionState { get; set; }
        public Account Account { get; set; }
        public Player Player { get; private set; }
        public int AdminLevel { get; set; }
        public int DonateRank { get; }
        public int Level { get; }
        public int Respect { get; }
        public int ConnectedTime { get; }
        public int HouseId { get; set; }
        public int Money { get; set; }
        public int Bank { get; }
        public GFHouse CurrentHouse { get; set; }
    }
}