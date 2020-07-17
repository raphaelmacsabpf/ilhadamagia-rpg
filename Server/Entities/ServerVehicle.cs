using Server.Enums;

namespace Server.Entities
{
    public class ServerVehicle
    {
        public ServerVehicle(VehicleHash hash, float spawnX, float spawnY, float spawnZ, float spawnHeading)
        {
            Hash = hash;
            SpawnX = spawnX;
            SpawnY = spawnY;
            SpawnZ = spawnZ;
            SpawnHeading = spawnHeading;
            Status = ServerVehicleStatus.NEW;
        }

        public int Handle { get; set; }
        public float SpawnX { get; set; }
        public float SpawnY { get; set; }
        public float SpawnZ { get; set; }
        public float SpawnHeading { get; set; }
        public VehicleHash Hash { get; set; }
        public ServerVehicleStatus Status { get; set; }
    }
}