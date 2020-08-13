using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using System.Collections.Generic;

namespace Server.Database
{
    public class OrgRepository
    {
        private readonly MySqlConnection mySqlConnection;

        public OrgRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetRandomConnection();
        }

        public IEnumerable<Org> GetAll()
        {
            return this.mySqlConnection.Query<Org>("SELECT * FROM imtb_org;");
        }
    }
}