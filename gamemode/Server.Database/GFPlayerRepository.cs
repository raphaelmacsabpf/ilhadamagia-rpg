using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using System.Collections.Generic;

namespace Server.Database
{
    public class GFPlayerRepository
    {
        private MySqlConnection mySqlConnection;

        public GFPlayerRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetOne();
        }

        public IEnumerable<GFPlayer> GetAll()
        {
            return this.mySqlConnection.Query<GFPlayer>("SELECT * FROM imtb_gm_player;");
        }
    }
}