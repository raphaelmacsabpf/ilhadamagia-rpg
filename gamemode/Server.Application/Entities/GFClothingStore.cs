using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GFClothingStore
    {
        public GFClothingStore(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; set; }
    }
}