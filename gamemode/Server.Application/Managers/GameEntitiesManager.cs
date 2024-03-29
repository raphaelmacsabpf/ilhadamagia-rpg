﻿using CitizenFX.Core;
using Server.Application.Entities;
using Server.Domain.Services;
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

        public event EventHandler<List<GFATM>> OnATMListLoad;

        public event EventHandler<List<GFClothingStore>> OnClothingStoresLoad;

        public event EventHandler<List<GFHospital>> OnHospitalsLoad;

        public event EventHandler<List<GFPoliceDepartment>> OnPoliceDepartmentsLoad;

        public event EventHandler<List<GF247Store>> On247StoresLoad;

        private readonly int maxOrgId;
        private readonly OrgService orgService;
        private readonly List<GFOrg> orgs;
        private readonly List<GFAmmunation> ammunations;
        private readonly List<GFGasStation> gasStations;
        private readonly List<GFATM> atmList;
        private readonly List<GFClothingStore> clothingStores;
        private readonly List<GFHospital> hospitals;
        private readonly List<GFPoliceDepartment> policeDepartments;
        private readonly List<GF247Store> store247List;

        public GameEntitiesManager(OrgService orgService)
        {
            this.orgService = orgService;
            this.orgs = new List<GFOrg>();
            this.ammunations = GetAmmunationsList();
            this.gasStations = GetGasStationsList();
            this.atmList = GetATMList();
            this.clothingStores = GetClothingStoreList();
            this.hospitals = GetHospitalList();
            this.policeDepartments = GetPoliceDepartmentList();
            this.store247List = Get247StoreList();

            var orgList = this.orgService.GetAllOrgs();
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
            OnATMListLoad?.Invoke(this, this.atmList);
            OnClothingStoresLoad?.Invoke(this, this.clothingStores);
            OnHospitalsLoad?.Invoke(this, this.hospitals);
            OnPoliceDepartmentsLoad?.Invoke(this, this.policeDepartments);
            On247StoresLoad?.Invoke(this, this.store247List);
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
            return new List<GFAmmunation>
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
            return new List<GFGasStation>
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

        private List<GFATM> GetATMList()
        {
            return new List<GFATM>
            {
                new GFATM(new Vector3(-386.733f, 6045.953f, 31.501f)),
                new GFATM(new Vector3(-110.753f, 6467.703f, 31.784f)),
                new GFATM(new Vector3(155.4300f, 6641.991f, 31.784f)),
                new GFATM(new Vector3(174.6720f, 6637.218f, 31.784f)),
                new GFATM(new Vector3(1703.138f, 6426.783f, 32.730f)),
                new GFATM(new Vector3(1735.114f, 6411.035f, 35.164f)),
                new GFATM(new Vector3(1702.842f, 4933.593f, 42.051f)),
                new GFATM(new Vector3(1967.333f, 3744.293f, 32.272f)),
                new GFATM(new Vector3(1174.532f, 2705.278f, 38.027f)),
                new GFATM(new Vector3(2564.399f, 2585.100f, 38.016f)),
                new GFATM(new Vector3(2558.683f, 349.6010f, 108.050f)),
                new GFATM(new Vector3(2558.051f, 389.4817f, 108.660f)),
                new GFATM(new Vector3(1077.692f, -775.796f, 58.218f)),
                new GFATM(new Vector3(1139.018f, -469.886f, 66.789f)),
                new GFATM(new Vector3(1168.975f, -457.241f, 66.641f)),
                new GFATM(new Vector3(1153.884f, -326.540f, 69.245f)),
                new GFATM(new Vector3(236.4638f, 217.4718f, 106.840f)),
                new GFATM(new Vector3(265.0043f, 212.1717f, 106.780f)),
                new GFATM(new Vector3(-164.568f, 233.5066f, 94.919f)),
                new GFATM(new Vector3(-1827.04f, 785.5159f, 138.020f)),
                new GFATM(new Vector3(-1409.39f, -99.2603f, 52.473f)),
                new GFATM(new Vector3(-1215.64f, -332.231f, 37.881f)),
                new GFATM(new Vector3(-2072.41f, -316.959f, 13.345f)),
                new GFATM(new Vector3(-2975.72f, 379.7737f, 14.992f)),
                new GFATM(new Vector3(-2962.60f, 482.1914f, 15.762f)),
                new GFATM(new Vector3(-3144.13f, 1127.415f, 20.868f)),
                new GFATM(new Vector3(-1305.40f, -706.240f, 25.352f)),
                new GFATM(new Vector3(-717.614f, -915.880f, 19.268f)),
                new GFATM(new Vector3(-526.566f, -1222.90f, 18.434f)),
                new GFATM(new Vector3(149.4551f, -1038.95f, 29.366f)),
                new GFATM(new Vector3(-846.304f, -340.402f, 38.687f)),
                new GFATM(new Vector3(-1216.27f, -331.461f, 37.773f)),
                new GFATM(new Vector3(-56.1935f, -1752.53f, 29.452f)),
                new GFATM(new Vector3(-273.001f, -2025.60f, 30.197f)),
                new GFATM(new Vector3(314.187f, -278.621f, 54.170f)),
                new GFATM(new Vector3(-351.534f, -49.529f, 49.042f)),
                new GFATM(new Vector3(-1570.197f, -546.651f, 34.955f)),
                new GFATM(new Vector3(33.232f, -1347.849f,29.497f)),
                new GFATM(new Vector3(129.216f, -1292.347f,29.269f)),
                new GFATM(new Vector3(289.012f, -1256.545f,29.440f)),
                new GFATM(new Vector3(1686.753f, 4815.809f, 42.008f)),
                new GFATM(new Vector3(-302.408f, -829.945f, 32.417f)),
                new GFATM(new Vector3(5.134f, -919.949f, 29.557f)),
                new GFATM(new Vector3(-284.037f, 6224.385f, 31.187f)),
                new GFATM(new Vector3(-135.165f, 6365.738f, 31.101f)),
                new GFATM(new Vector3(-94.9690f, 6455.301f, 31.784f)),
                new GFATM(new Vector3(1821.917f, 3683.483f, 34.244f)),
                new GFATM(new Vector3(540.0420f, 2671.007f, 42.177f)),
                new GFATM(new Vector3(381.2827f, 323.2518f, 103.270f)),
                new GFATM(new Vector3(285.2029f, 143.5690f, 104.970f)),
                new GFATM(new Vector3(157.7698f, 233.5450f, 106.450f)),
                new GFATM(new Vector3(-1205.35f, -325.579f, 37.870f)),
                new GFATM(new Vector3(-2955.70f, 488.7218f, 15.486f)),
                new GFATM(new Vector3(-3044.22f, 595.2429f, 7.595f)),
                new GFATM(new Vector3(-3241.10f, 996.6881f, 12.500f)),
                new GFATM(new Vector3(-3241.11f, 1009.152f, 12.877f)),
                new GFATM(new Vector3(-538.225f, -854.423f, 29.234f)),
                new GFATM(new Vector3(-711.156f, -818.958f, 23.768f)),
                new GFATM(new Vector3(-256.831f, -719.646f, 33.444f)),
                new GFATM(new Vector3(-203.548f, -861.588f, 30.205f)),
                new GFATM(new Vector3(112.4102f, -776.162f, 31.427f)),
                new GFATM(new Vector3(112.9290f, -818.710f, 31.386f)),
                new GFATM(new Vector3(119.9000f, -883.826f, 31.191f)),
                new GFATM(new Vector3(-261.692f, -2012.64f, 30.121f)),
                new GFATM(new Vector3(-254.112f, -692.483f, 33.616f)),
                new GFATM(new Vector3(-1415.909f,-211.825f, 46.500f)),
                new GFATM(new Vector3(-1430.122f,-211.014f, 46.500f)),
                new GFATM(new Vector3(287.645f, -1282.646f,29.659f)),
                new GFATM(new Vector3(295.839f, -895.640f, 29.217f)),
                new GFATM(new Vector3(-1315.73f, -834.89f, 16.96f)),
                new GFATM(new Vector3(89.75f, 2.35f, 68.31f)),
            };
        }

        private List<GFClothingStore> GetClothingStoreList()
        {
            return new List<GFClothingStore>
            {
                new GFClothingStore(new Vector3(72.2545394897461f, -1399.10229492188f, 29.3761386871338f)),
                new GFClothingStore(new Vector3(-703.77685546875f, -152.258544921875f, 37.4151458740234f)),
                new GFClothingStore(new Vector3(-167.863754272461f, -298.969482421875f, 39.7332878112793f)),
                new GFClothingStore(new Vector3(428.694885253906f, -800.1064453125f, 29.4911422729492f)),
                new GFClothingStore(new Vector3(-829.413269042969f, -1073.71032714844f, 11.3281078338623f)),
                new GFClothingStore(new Vector3(-1447.7978515625f, -242.461242675781f, 49.8207931518555f)),
                new GFClothingStore(new Vector3(11.6323690414429f, 6514.224609375f, 31.8778476715088f)),
                new GFClothingStore(new Vector3(123.64656829834f, -219.440338134766f, 54.5578384399414f)),
                new GFClothingStore(new Vector3(1696.29187011719f, 4829.3125f, 42.0631141662598f)),
                new GFClothingStore(new Vector3(618.093444824219f, 2759.62939453125f, 42.0881042480469f)),
                new GFClothingStore(new Vector3(1190.55017089844f, 2713.44189453125f, 38.2226257324219f)),
                new GFClothingStore(new Vector3(-1193.42956542969f, -772.262329101563f, 17.3244285583496f)),
                new GFClothingStore(new Vector3(-3172.49682617188f, 1048.13330078125f, 20.8632030487061f)),
                new GFClothingStore(new Vector3(1108.44177246094f, 2708.92358398438f, 19.1078643798828f))
            };
        }

        private List<GFHospital> GetHospitalList()
        {
            return new List<GFHospital>
            {
                new GFHospital(new Vector3(1839.41f, 3672.90f, 34.28f)),
                new GFHospital(new Vector3(-247.76f, 6331.23f, 32.43f)),
                new GFHospital(new Vector3(-449.67f, -340.83f, 34.50f)),
                new GFHospital(new Vector3(357.43f, -593.36f, 28.79f)),
                new GFHospital(new Vector3(295.83f, -1446.94f, 29.97f)),
                new GFHospital(new Vector3(-676.98f, 310.68f, 83.08f)),
                new GFHospital(new Vector3(1151.21f, -1529.62f, 35.37f)),
                new GFHospital(new Vector3(-874.64f, -307.71f, 39.58f))
            };
        }

        private List<GFPoliceDepartment> GetPoliceDepartmentList()
        {
            return new List<GFPoliceDepartment>
            {
                new GFPoliceDepartment("La Mesa", new Vector3(826.6022f, -1289.947f, 28.23511f)),
                new GFPoliceDepartment("Mission Row", new Vector3(441.0725f, -981.1384f, 30.67834f)),
                new GFPoliceDepartment("Sandy Shore", new Vector3(1853.96f, 3688.18f, 34.25061f)),
                new GFPoliceDepartment("Paleto Bay", new Vector3(-447.7846f, 6013.701f, 31.70618f)),
                new GFPoliceDepartment("Vespucci", new Vector3(-1093.015f, -809.578f, 19.27112f)),
                new GFPoliceDepartment("Davis", new Vector3(360.8835f, -1584.567f, 29.27991f))
            };
        }

        private List<GF247Store> Get247StoreList()
        {
            return new List<GF247Store>()
            {
                new GF247Store(new Vector3(1735.83f, 6419.51f, 35.0373f)),
                new GF247Store(new Vector3(1960.42f, 3748.89f, 32.3438f)),
                new GF247Store(new Vector3(2682.55f, 3282.32f, 55.24f)),
                new GF247Store(new Vector3(1700.17f, 4927.65f, 46.93f)),
                new GF247Store(new Vector3(-2974.73f, 390.80f, 22.50f)),
                new GF247Store(new Vector3(29.15f, -1344.53f, 36.23f)),
                new GF247Store(new Vector3(-1224.34f, -905.99f, 19.47f)),
                new GF247Store(new Vector3(1394.169189f, 3599.860107f, 34.012100f)),
                new GF247Store(new Vector3(-3038.908203f, 589.518677f, 6.904800f)),
                new GF247Store(new Vector3(-3240.316895f, 1004.433411f, 11.830700f)),
                new GF247Store(new Vector3(544.280212f, 2672.811279f, 41.156601f)),
                new GF247Store(new Vector3(2559.247070f, 385.526611f, 107.623001f)),
                new GF247Store(new Vector3(376.653290f, 323.647095f, 102.566399f)),
                new GF247Store(new Vector3(1166.391968f, 2703.504150f, 37.157299f)),
                new GF247Store(new Vector3(-2973.261719f, 390.818390f, 14.043300f)),
                new GF247Store(new Vector3(-1491.056519f, -383.572815f, 39.170601f)),
                new GF247Store(new Vector3(1698.808472f, 4929.197754f, 41.078300f)),
                new GF247Store(new Vector3(-711.721008f, -916.696472f, 18.214500f)),
                new GF247Store(new Vector3(-53.124001f, -1756.405396f, 28.421000f)),
                new GF247Store(new Vector3(1159.542114f, -326.698608f, 67.922997f)),
                new GF247Store(new Vector3(-1822.286621f, 788.005981f, 137.185898f)),
            };
        }
    }
}