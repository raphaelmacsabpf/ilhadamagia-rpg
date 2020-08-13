using System;

namespace Server.Domain.Entities
{
    public class Account
    {
        public Account()
        {
        }

        public int Id { get; set; }
        public string Guid { get; set; }
        public string License { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int AdminLevel { get; set; }
        public int DonateRank { get; set; }
        public int Level { get; set; }
        public int Respect { get; set; }
        public int ConnectedTime { get; set; }
        public int Money { get; set; }
        public int Bank { get; set; }
        public string PedModel { get; set; }
        public int? SelectedHouse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public float LastX { get; set; }
        public float LastY { get; set; }
        public float LastZ { get; set; }
        public int? LastHouseInside { get; set; }
        public int OrgId { get; set; }
    }
}