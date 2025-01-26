using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Database.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MySqlConnection mySqlConnection;

        public VehicleRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetRandomConnection();
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return this.mySqlConnection.Query<Vehicle>("SELECT * FROM imtb_vehicle;");
        }

        public IEnumerable<Vehicle> GetAccountVehicles(Account owner)
        {
            return this.mySqlConnection.Query<Vehicle>(@"SELECT * FROM imtb_vehicle WHERE Owner=@Owner;", new { Owner = owner.Username });
        }

        public Task Create(Vehicle vehicle)
        {
            const string sqlStatement = @"
                INSERT INTO imtb_vehicle
                (Owner, Hash, PrimaryColor, SecondaryColor, Fuel, EngineHealth)
                VALUES
                (@Owner, @Hash, @PrimaryColor, @SecondaryColor, @Fuel, @EngineHealth);";

            var values = new
            {
                vehicle.Owner,
                vehicle.Hash,
                vehicle.PrimaryColor,
                vehicle.SecondaryColor,
                vehicle.Fuel,
                vehicle.EngineHealth,
            };

            return this.mySqlConnection.ExecuteAsync(sqlStatement, values);
        }
    }
}