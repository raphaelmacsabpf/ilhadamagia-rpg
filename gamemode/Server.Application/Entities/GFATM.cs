using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GFATM
    {
        public GFATM(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; set; }
    }
}