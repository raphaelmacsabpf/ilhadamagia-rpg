namespace GF.CrossCutting.Dto.MenuActions
{
    public class CallHouseVehicleDto
    {
        public CallHouseVehicleDto(int vehicleId)
        {
            VehicleId = vehicleId;
        }

        public int VehicleId { get; set; }
    }
}
