using Server.Application.Entities;
using Server.Database;
using System.Collections.Generic;
using System.Linq;

namespace Server.Application.Managers
{
    public class GameEntitiesManager
    {
        private readonly int maxOrgId;
        private readonly OrgRepository orgRepository;
        private readonly List<GFOrg> orgs;

        public GameEntitiesManager(OrgRepository orgRepository)
        {
            this.orgRepository = orgRepository;
            this.orgs = new List<GFOrg>();
            var orgList = orgRepository.GetAll();
            maxOrgId = 0;
            foreach (var orgEntity in orgList)
            {
                this.orgs.Add(new GFOrg()
                {
                    Entity = orgEntity
                });

                if (orgEntity.Id > maxOrgId)
                {
                    maxOrgId = orgEntity.Id;
                }
            }
        }

        public int GetMaxOrgId()
        {
            return this.maxOrgId;
        }

        public GFOrg GetGFOrgById(int orgId)
        {
            return this.orgs.FirstOrDefault(x => x.Entity.Id == orgId);
        }
    }
}