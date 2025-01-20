using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Database.Repositories
{
    public class HouseRepository : IHouseRepository
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

        public IEnumerable<House> GetAllFromAccount(Account account)
        {
            var sqlStatement = @"
SELECT * FROM imtb_house
WHERE Owner=@Owner;";
            return this.mySqlConnection.Query<House>(sqlStatement, new { Owner = account.Username });
        }

        public Task Create(string Owner, float EntranceX, float EntranceY, float EntranceZ, PropertyType PropertyType, PropertySellState SellState, InteriorType InteriorType, float VehiclePositionX, float VehiclePositionY, float VehiclePositionZ, float VehicleHeading)
        {
            const string sqlStatement = @"
                INSERT INTO imtb_house
                (Owner, EntranceX, EntranceY, EntranceZ, PropertyType, SellState, Interior, VehiclePositionX, VehiclePositionY, VehiclePositionZ, VehicleHeading)
                VALUES
                (@Owner, @EntranceX, @EntranceY, @EntranceZ, @PropertyType, @SellState, @Interior, @VehiclePositionX, @VehiclePositionY, @VehiclePositionZ, @VehicleHeading);";

            var values = new
            {
                Owner,
                EntranceX,
                EntranceY,
                EntranceZ,
                PropertyType,
                SellState,
                Interior = InteriorType,
                VehiclePositionX,
                VehiclePositionY,
                VehiclePositionZ,
                VehicleHeading
            };

            return this.mySqlConnection.ExecuteAsync(sqlStatement, values);
        }
    }
}