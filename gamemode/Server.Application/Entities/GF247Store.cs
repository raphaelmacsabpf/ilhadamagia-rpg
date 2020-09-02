using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GF247Store
    {
        public GF247Store(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; set; }
    }
}