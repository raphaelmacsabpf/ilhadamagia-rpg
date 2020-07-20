using CitizenFX.Core;
using Server.Enums;

namespace Server.Entities
{
    public class GFHouse : GFProperty
    {
        public GFHouse(int globalId, Vector3 entrance, Vector4 vehiclePosition)
        {
            this.PropertyType = PropertyType.House;
            this.Interior = InteriorType.LOW_END_APARTMENT;
            this.GlobalId = globalId;
            this.Entrance = entrance;
            this.VehiclePosition = vehiclePosition;
            this.SellState = PropertySellState.FOR_SALE;
        }

        public int GlobalId { get; set; }
        public InteriorType Interior { get; set; }
        public Vector4 VehiclePosition { get; set; }
    }
}