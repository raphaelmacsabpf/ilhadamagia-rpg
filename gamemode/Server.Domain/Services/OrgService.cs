using Server.Domain.Entities;
using Server.Domain.Interfaces;
using System.Collections.Generic;

namespace Server.Domain.Services
{
    public class OrgService
    {
        private readonly IOrgRepository orgRepository;

        public OrgService(IOrgRepository orgRepository)
        {
            this.orgRepository = orgRepository;
        }

        public IEnumerable<Org> GetAllOrgs()
        {
            return orgRepository.GetAll();
        }
    }
}