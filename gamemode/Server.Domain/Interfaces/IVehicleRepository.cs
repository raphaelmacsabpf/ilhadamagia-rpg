using Server.Domain.Entities;
using System.Collections.Generic;

namespace Server.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAccountVehicles(Account owner);

        IEnumerable<Vehicle> GetAll();
    }
}