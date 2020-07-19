using CitizenFX.Core;
using Server.Enums;

namespace Server.Entities
{
    public abstract class GFProperty
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public Vector3 Entrance { get; set; }
        public PropertyType PropertyType { get; set; }
        public PropertySellState SellState { get; set; }
    }
}