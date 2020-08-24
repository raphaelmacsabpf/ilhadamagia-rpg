using CitizenFX.Core;

namespace Server.Application.Entities
{
    public class GFAmmunation
    {
        // TODO: Amarrar ammunation a uma empresa
        public GFAmmunation(string name, Vector3 shopCounterPosition)
        {
            this.ShopCounterPosition = shopCounterPosition;
            this.Name = name;
        }

        public Vector3 ShopCounterPosition { get; }
        public string Name { get; }
    }
}