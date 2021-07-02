using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using Server.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Repositories
{
    public class OrgRepository : IOrgRepository
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

        public Task Update(Org org)
        {
            const string sqlStatement = @"
UPDATE imtb_org SET
Leader = @Leader,
SpawnX = @SpawnX,
SpawnY = @SpawnY,
SpawnZ = @SpawnZ
WHERE Id = @Id";
            return this.mySqlConnection.ExecuteAsync(sqlStatement, org);
        }

        public IEnumerable<string> GetOrgMembersById(int orgId)
        {
            return this.mySqlConnection.Query<string>(""); // TODO: Implement GetOrgMembers query
        }

        public Org GetOrgFromUsername(string username)
        {
            return this.mySqlConnection.Query<Org>("SELECT * FROM imtb_org o JOIN imtb_player_orgs po ON o.Id = po.OrgId WHERE po.Username = @Username LIMIT 1", new { Username = username }).FirstOrDefault();
        }
    }
}