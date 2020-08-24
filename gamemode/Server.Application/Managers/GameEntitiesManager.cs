using CitizenFX.Core;
using Server.Application.Entities;
using Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Application.Managers
{
    public class GameEntitiesManager
    {
        public event EventHandler<List<GFOrg>> OnOrgsLoad;

        public event EventHandler<List<GFAmmunation>> OnAmmunationsLoad;

        private readonly int maxOrgId;
        private readonly OrgRepository orgRepository;
        private readonly List<GFOrg> orgs;
        private readonly List<GFAmmunation> ammunations;

        public GameEntitiesManager(OrgRepository orgRepository)
        {
            this.orgRepository = orgRepository;
            this.orgs = new List<GFOrg>();
            this.ammunations = GetAmmunationsList();
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

        public void InvokeInitialEvents()
        {
            OnOrgsLoad?.Invoke(this, this.orgs);
            OnAmmunationsLoad?.Invoke(this, this.ammunations);
        }

        public int GetMaxOrgId()
        {
            return this.maxOrgId;
        }

        public GFOrg GetGFOrgById(int orgId)
        {
            return this.orgs.FirstOrDefault(x => x.Entity.Id == orgId);
        }

        private static List<GFAmmunation> GetAmmunationsList()
        {
            return new List<GFAmmunation>()
            {
                new GFAmmunation("Polito Bay", new Vector3(-332.2681f, 6082.47f, 31.45349f)),
                new GFAmmunation("Sandy Shores", new Vector3(1692.105f, 3758.862f, 34.6886f)),
                new GFAmmunation("Great Chaparral", new Vector3(-1119.297f, 2697.666f, 18.54651f)),
                new GFAmmunation("Chumash", new Vector3(-3173.064f, 1086.079f, 20.83813f)),
                new GFAmmunation("Palomino Fwy", new Vector3(2569.846f, 294.0791f, 108.7267f)),
                new GFAmmunation("Hawick", new Vector3(253.1736f, -48.22417f, 69.93848f)),
                new GFAmmunation("Morningwood", new Vector3(-1305.086f, -392.9011f, 36.69373f)),
                new GFAmmunation("Little Seoul", new Vector3(-663.7451f, -934.9319f, 21.81543f)),
                new GFAmmunation("La Mesa", new Vector3(844.0747f, -1033.938f, 28.18457f)),
                new GFAmmunation("Pillbox Hill", new Vector3(20.17583f, -1106.083f, 29.7854f)),
                new GFAmmunation("Cypress Flats", new Vector3(811.8329f, -2157.679f, 29.61682f))
            };
        }
    }
}