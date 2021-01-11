using System;

namespace Server.Domain.Entities
{
    public class Account
    {
        public Account()
        {
        }

        public int Id { get; }
        public string Guid { get; }
        public string License { get; }
        public string Username { get; }
        public string Password { get; }
        public int AdminLevel { get; private set; }
        public int DonateRank { get; }
        public int Level { get; }
        public int Respect { get; }
        public int ConnectedTime { get; }
        public long Money { get; private set; }
        public long Bank { get; private set; }
        public string PedModel { get; private set; }
        public int? SelectedHouse { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }
        public float LastX { get; private set; }
        public float LastY { get; private set; }
        public float LastZ { get; private set; }
        public int? LastHouseInside { get; private set; }
        public bool IsLeader { get; }

        public void GiveMoney(long ammount)
        {
            this.Money += ammount;
        }

        public void TakeMoney(long ammount)
        {
            this.Money -= ammount;
        }

        public void DepositToBank(long ammount)
        {
            this.Money -= ammount;
            this.Bank += ammount;
        }

        public void WithdrawFromBank(long ammount)
        {
            this.Money += ammount;
            this.Bank -= ammount;
        }

        public void SetAdminLevel(int adminLevel)
        {
            this.AdminLevel = adminLevel;
        }

        public void SetPedModel(string pedModel)
        {
            this.PedModel = pedModel;
        }

        public void SetSelectedHouse(House house)
        {
            this.SelectedHouse = house.Id;
        }

        public void EndSession(float x, float y, float z, House house)
        {
            this.LastX = x;
            this.LastY = y;
            this.LastZ = z;

            this.LastHouseInside = house != null ? Convert.ToInt32(house.Id) : (int?)null;
            this.UpdatedAt = DateTime.Now;
        }
    }
}