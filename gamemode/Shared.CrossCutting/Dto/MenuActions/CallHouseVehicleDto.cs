namespace GF.CrossCutting.Dto.MenuActions
{
    public class CallHouseVehicleDto
    {
        public CallHouseVehicleDto(string vehicleGuid)
        {
            VehicleGuid = vehicleGuid;
        }

        public string VehicleGuid { get; set; }
    }
}
