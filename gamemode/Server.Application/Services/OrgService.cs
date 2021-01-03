using Server.Database;
using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Application.Services
{
    public class OrgService
    {
        private readonly OrgRepository orgRepository;

        public OrgService(OrgRepository orgRepository)
        {
            this.orgRepository = orgRepository;
        }

        public IEnumerable<Org> GetAllOrgs()
        {
            return orgRepository.GetAll();
        } 
    }
}
