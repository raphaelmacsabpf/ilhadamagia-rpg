namespace Server.Domain.Entities
{
    public class The247Store
    {
        public The247Store(float positionX, float positionY, float positionZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
    }
}