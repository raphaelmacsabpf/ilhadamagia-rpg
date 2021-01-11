using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using Server.Domain.Interfaces;
using System.Collections.Generic;

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
    }
}