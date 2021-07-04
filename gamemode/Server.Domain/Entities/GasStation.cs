namespace Server.Domain.Entities
{
    public class GasStation
    {
        public GasStation(string name, float positionX, float positionY, float positionZ)
        {
            Name = name;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public string Name { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
    }
}