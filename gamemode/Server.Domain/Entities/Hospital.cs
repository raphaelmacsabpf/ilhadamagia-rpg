namespace Server.Domain.Entities
{
    public class Hospital
    {
        public Hospital(float positionX, float positionY, float positionZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }
    }
}