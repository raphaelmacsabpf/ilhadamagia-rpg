using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class House
    {
        public House()
        {
        }

        public int Id { get; set; }
        public string Guid { get; set; }
        public string Owner { get; set; }
        public float EntranceX { get; set; }
        public float EntranceY { get; set; }
        public float EntranceZ { get; set; }
        public PropertySellState SellState { get; set; }
        public InteriorType Interior { get; set; }
        public float VehiclePositionX { get; set; }
        public float VehiclePositionY { get; set; }
        public float VehiclePositionZ { get; set; }
        public float VehicleHeading { get; set; }
    }
}