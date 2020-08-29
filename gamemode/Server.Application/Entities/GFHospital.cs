using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GFHospital
    {
        public Vector3 Position { get; }

        public GFHospital(Vector3 position)
        {
            Position = position;
        }
    }
}
