using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public abstract class GFProperty
    {
        public string Owner { get; set; }
        public float EntranceX { get; set; }
        public float EntranceY { get; set; }
        public float EntranceZ { get; set; }
        public PropertyType PropertyType { get; set; }
        public PropertySellState SellState { get; set; }
    }
}