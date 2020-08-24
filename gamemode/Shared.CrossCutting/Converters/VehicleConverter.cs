using GF.CrossCutting.Enums;
using System;
using System.Collections.Generic;

namespace GF.CrossCutting.Converters
{
    public static class VehicleConverter
    {
        private static readonly Dictionary<GameVehicleHash, string> vehicleNames;

        static VehicleConverter()
        {
            vehicleNames = new Dictionary<GameVehicleHash, string>()
            {
                // TODO: Criar nomes customizados para os veículos
            };
        }

        public static string GetVehicleName(GameVehicleHash vehicleHash)
        {
            if (vehicleNames.TryGetValue(vehicleHash, out var customVehicleName))
            {
                return customVehicleName;
            }

            var defaultVehicleName = vehicleHash.ToString();
            return defaultVehicleName;
        }

        public static GameVehicleHash GetVehicleHashById(int id)
        {
            Array values = Enum.GetValues(typeof(GameVehicleHash));
            var vehicleHash = (GameVehicleHash)values.GetValue(id);
            return vehicleHash;
        }

        public static string GetVehicleNameById(int id)
        {
            Array values = Enum.GetValues(typeof(GameVehicleHash));
            var vehicleHash = (GameVehicleHash)values.GetValue(id);

            if (vehicleNames.TryGetValue(vehicleHash, out var customVehicleName))
            {
                return customVehicleName;
            }

            var defaultWeaponName = vehicleHash.ToString();
            return defaultWeaponName;
        }

        public static int GetVehicleMaxId()
        {
            return Enum.GetValues(typeof(GameVehicleHash)).Length - 1; // TODO: avaliar se esse menos 1 é valido
        }
    }
}