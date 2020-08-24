using GF.CrossCutting.Enums;
using System;
using System.Collections.Generic;

namespace GF.CrossCutting.Converters
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

        public static string GetWeaponNameById(int id)
        {
            Array values = Enum.GetValues(typeof(GameWeaponHash));
            var weaponHash = (GameWeaponHash)values.GetValue(id);

            if (weaponCustomNames.TryGetValue(weaponHash, out var customWeaponName))
            {
                return customWeaponName;
            }

            var defaultWeaponName = weaponHash.ToString();
            return defaultWeaponName;
        }

        public static int GetWeaponMaxId()
        {
            return Enum.GetValues(typeof(GameWeaponHash)).Length - 1; // TODO: avaliar se esse menos 1 é valido
        }
    }
}