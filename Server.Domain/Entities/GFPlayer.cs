namespace Server.Domain.Entities
{
    public class GFPlayer
    {
        public GFPlayer(int playerId, int globalId) // TODO: Rename GFPlayer to GMPlayer
        {
            this.PlayerId = playerId;
            this.GlobalId = globalId;
            this.Username = ""; // TODO: Set sanitized player.Name as GFPlayer.Username
        }

        public int GlobalId { get; set; }
        public bool IsActive { get; set; }
        public int PlayerId { get; }

        public string Username { get; set; }

        public string Password { get; }
        public string License { get; }
        public int AdminLevel { get; set; }
        public int DonateRank { get; }
        public int Level { get; }
        public int Respect { get; }
        public int ConnectedTime { get; }
        public int HouseId { get; set; }
        public int Money { get; set; }
        public int Bank { get; }
        public GFHouse CurrentHouse { get; set; } // HACK: Não salvar no DB
    }
}