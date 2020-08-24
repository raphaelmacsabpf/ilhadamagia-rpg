using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GFGasStation
    {
        public GFGasStation(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }

        public string Name { get; set; }
        public Vector3 Position { get; set; }
    }
}