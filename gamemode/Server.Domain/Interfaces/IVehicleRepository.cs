using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAccountVehicles(Account owner);

        IEnumerable<Vehicle> GetAll();

        Task Create(Vehicle vehicle);
    }
}