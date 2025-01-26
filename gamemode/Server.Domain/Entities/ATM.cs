namespace Server.Domain.Entities
{
    public class ATM
    {
        public ATM(float positionX, float positionY, float positionZ)
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