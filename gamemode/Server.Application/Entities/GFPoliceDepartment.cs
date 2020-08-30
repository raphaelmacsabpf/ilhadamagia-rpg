using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GFPoliceDepartment
    {
        public string Name { get; }
        public Vector3 Position { get; }

        public GFPoliceDepartment(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }
    }
}