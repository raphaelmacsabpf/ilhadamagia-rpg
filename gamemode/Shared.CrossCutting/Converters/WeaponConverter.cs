using Shared.CrossCutting.Enums;
using System;
using System.Collections.Generic;

namespace Shared.CrossCutting.Converters
{
    public static class WeaponConverter
    {
        private static readonly Dictionary<GameWeaponHash, string> weaponCustomNames;

        static WeaponConverter()
        {
            weaponCustomNames = new Dictionary<GameWeaponHash, string>()
            {
                // TODO: Criar nomes customizados para as armas
            };
        }

        public static GameWeaponHash GetWeaponHashById(int id)
        {
            Array values = Enum.GetValues(typeof(GameWeaponHash));
            var weaponHash = (GameWeaponHash)values.GetValue(id);
            return weaponHash;
        }

        public static string GetWeaponName(GameWeaponHash weaponHash)
        {
            if (weaponCustomNames.TryGetValue(weaponHash, out var customVehicleName))
            {
                return customVehicleName;
            }

            var defaultWeaponName = weaponHash.ToString();
            return defaultWeaponName;
        }
    }
}