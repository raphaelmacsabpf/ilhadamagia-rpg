using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Database
{
    public class HouseRepository
    {
        private readonly MySqlConnection mySqlConnection;

        public HouseRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetRandomConnection();
        }

        public IEnumerable<House> GetAll()
        {
            return this.mySqlConnection.Query<House>("SELECT * FROM imtb_house;");
        }

        public Task<IEnumerable<House>> GetAllFromAccount(Account account)
        {
            var sqlStatement = @"
SELECT * FROM imtb_house
WHERE Owner=@Owner;";
            return this.mySqlConnection.QueryAsync<House>(sqlStatement, new { Owner = account.Username });
        }
    }
}