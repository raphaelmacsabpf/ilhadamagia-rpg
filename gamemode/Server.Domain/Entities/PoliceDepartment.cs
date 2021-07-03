namespace Server.Domain.Entities
{
    public class PoliceDepartment
    {
        public string Name { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }

        public PoliceDepartment(string name, float positionX, float positionY, float positionZ)
        {
            Name = name;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }
    }
}