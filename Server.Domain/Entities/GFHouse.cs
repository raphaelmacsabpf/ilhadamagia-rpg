using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class GFHouse : GFProperty
    {
        public GFHouse(int globalId, float entranceX, float entranceY, float entranceZ, float vehiclePositionX, float vehiclePositionY, float vehiclePositionZ, float vehicleHeading)
        {
            this.PropertyType = PropertyType.House;
            this.Interior = InteriorType.LOW_END_APARTMENT;
            this.GlobalId = globalId;
            this.EntranceX = entranceX;
            this.EntranceY = entranceY;
            this.EntranceZ = entranceZ;
            this.VehiclePositionX = vehiclePositionX;
            this.VehiclePositionY = vehiclePositionY;
            this.VehiclePositionZ = vehiclePositionZ;
            this.VehicleHeading = vehicleHeading;
            this.SellState = PropertySellState.FOR_SALE;
        }

        public int GlobalId { get; set; }
        public InteriorType Interior { get; set; }
        public float VehiclePositionX { get; set; }
        public float VehiclePositionY { get; set; }
        public float VehiclePositionZ { get; set; }
        public float VehicleHeading { get; set; }
    }
}