using Server.Domain.Entities;
using Server.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IHouseRepository
    {
        IEnumerable<House> GetAll();

        IEnumerable<House> GetAllFromAccount(Account account);

        Task Create(string Owner, float EntranceX, float EntranceY, float EntranceZ, PropertyType PropertyType, PropertySellState SellState, InteriorType InteriorType, float VehiclePositionX, float VehiclePositionY, float VehiclePositionZ, float VehicleHeading);
    }
}
