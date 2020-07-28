using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using System.Collections.Generic;

namespace Server.Database
{
    public class HouseRepository
    {
        private MySqlConnection mySqlConnection;

        public HouseRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetRandomConnection();
        }

        public IEnumerable<House> GetAll()
        {
            return this.mySqlConnection.Query<House>("SELECT * FROM imtb_house;");
        }
    }
}