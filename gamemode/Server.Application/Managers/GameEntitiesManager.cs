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
        public event EventHandler<List<GFGasStation>> OnGasStationsLoad;

        private readonly int maxOrgId;
        private readonly OrgRepository orgRepository;
        private readonly List<GFOrg> orgs;
        private readonly List<GFAmmunation> ammunations;
        private readonly List<GFGasStation> gasStations;

        public GameEntitiesManager(OrgRepository orgRepository)
        {
            this.orgRepository = orgRepository;
            this.orgs = new List<GFOrg>();
            this.ammunations = GetAmmunationsList();
            this.gasStations = GetGasStationsList();
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
            OnGasStationsLoad?.Invoke(this, this.gasStations);
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

        private List<GFGasStation> GetGasStationsList()
        {
            return new List<GFGasStation>()
            {
                new GFGasStation("Little Seoul 1", new Vector3(-731.3538f, -932.4659f, 19.01831f)),
                new GFGasStation("Chamberlain Hills", new Vector3(-68.03076f, -1758.422f, 29.39783f)),
                new GFGasStation("Strawberry", new Vector3(266.3736f, -1253.328f, 29.12817f)),
                new GFGasStation("La Mesa", new Vector3(809.4857f, -1026.343f, 26.24683f)),
                new GFGasStation("Pacific Bluffs", new Vector3(-2095.503f, -320.2813f, 13.01978f)),
                new GFGasStation("Grand Senora 1", new Vector3(1208.163f, 2657.921f, 37.80591f)),
                new GFGasStation("Grand Senora 2", new Vector3(2679.6f, 3261.481f, 55.22852f)),
                new GFGasStation("Lago_Zancudo", new Vector3(-2558.677f, 2340.158f, 33.07104f)),
                new GFGasStation("Paleto Bay 1", new Vector3(180.9626f, 6605.328f, 31.85791f)),
                new GFGasStation("Tataviam Mountains", new Vector3(2589.943f, 364.0088f, 108.4572f)),
                new GFGasStation("Mount Chiliad", new Vector3(1702.154f, 6417.468f, 32.63293f)),
                new GFGasStation("Richman Glen", new Vector3(-1801.332f, 807.033f, 138.4835f)),
                new GFGasStation("Paleto Bay 2", new Vector3(-90.43517f, 6421.767f, 31.48718f)),
                new GFGasStation("Harmony", new Vector3(265.2527f, 2605.846f, 44.84912f)),
                new GFGasStation("Grand Senora 3", new Vector3(49.41099f, 2780.387f, 57.87402f)),
                new GFGasStation("Ron Alternates", new Vector3(2538.303f, 2594.202f, 37.94067f)),
                new GFGasStation("Mirror Park", new Vector3(2538.303f, 2594.202f, 37.94067f)),
                new GFGasStation("Little Seoul 2", new Vector3(-519.0198f, -1210.813f, 18.17578f)),
                new GFGasStation("El Burro Heights", new Vector3(1204.998f, -1401.837f, 35.21094f)),
                new GFGasStation("Sandy Shores", new Vector3(2000.769f, 3773.406f, 32.17798f)),
                new GFGasStation("Downtown Vinewood", new Vector3(619.7802f, 273.9165f, 103.082f)),
                new GFGasStation("Morningwood", new Vector3(-1443.824f, -273.4813f, 46.21387f)),
                new GFGasStation("Grapeseed", new Vector3(1689.046f, 4928.505f, 42.06885f)),
                new GFGasStation("Davis", new Vector3(181.1341f, -1561.345f, 29.3136f))
            };
        }
    }
}