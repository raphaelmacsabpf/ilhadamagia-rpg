namespace Server.Domain.Entities
{
    public class Ammunation
    {
        public Ammunation(string name, float positionX, float positionY, float positionZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
            Name = name;
        }

        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }
        public string Name { get; }
    }
}