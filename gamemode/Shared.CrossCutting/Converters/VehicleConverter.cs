using Shared.CrossCutting.Enums;
using System;
using System.Collections.Generic;

namespace Shared.CrossCutting.Converters
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
    }
}