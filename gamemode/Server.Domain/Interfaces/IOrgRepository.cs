using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IOrgRepository
    {
        IEnumerable<Org> GetAll();

        IEnumerable<OrgMembership> GetOrgMembers(string orgId);

        Org GetOrgById(string orgId);

        Task Update(Org org);

        Org GetOrgFromUsername(string username);

        void SetPlayerOrg(string username, string orgId, int role);
    }
}