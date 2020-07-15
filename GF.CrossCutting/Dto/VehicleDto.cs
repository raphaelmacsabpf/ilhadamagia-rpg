namespace GF.CrossCutting.Dto
{
    // CreateVehicle(Model model, Vector3 position, float heading = 0);
    public class VehicleDto
    {
        public VehicleDto(int vehicleHashInt, float x, float y, float z, float heading)
        {
            VehicleHashInt = vehicleHashInt;
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
        }

        public int VehicleHashInt { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }
    }
}