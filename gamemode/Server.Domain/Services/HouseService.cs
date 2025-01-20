using System.Collections.Generic;
using Server.Domain.Entities;
using Server.Domain.Interfaces;

namespace Server.Domain.Services
{
    public class HouseService
    {
        private readonly IHouseRepository houseRepository;
        
        public HouseService(IHouseRepository houseRepository)
        {
            this.houseRepository = houseRepository;
        }

        public IEnumerable<House> GetAll() 
        {
            return this.houseRepository.GetAll();
        }

        public void Create(House house)
        {
            this.houseRepository.Create(house.Owner, house.EntranceX, house.EntranceY, house.EntranceZ, house.PropertyType, house.SellState, house.Interior, house.VehiclePositionX, house.VehiclePositionY, house.VehiclePositionZ, house.VehicleHeading);
        }

        public IEnumerable<House> GetAllFromAccount(Account account)
        {
            return this.houseRepository.GetAllFromAccount(account);
        }
    }
}
