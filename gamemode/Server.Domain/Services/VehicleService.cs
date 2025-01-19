using Server.Domain.Entities;
using Server.Domain.Interfaces;
using System.Collections.Generic;

namespace Server.Domain.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            this.vehicleRepository = vehicleRepository;
        }

        public IEnumerable<Vehicle> GetAccountVehicles(Account account)
        {
            return this.vehicleRepository.GetAccountVehicles(account);
        }

        public void Create(Vehicle vehicle)
        {
            this.vehicleRepository.Create(vehicle);
        }
    }
}