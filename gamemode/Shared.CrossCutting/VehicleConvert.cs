using Shared.CrossCuting.Domain.Enums;
using System.Collections.Generic;

namespace GF.CrossCutting
{
    public static class VehicleConvert
    {
        private static Dictionary<DomainVehicleHash, string> vehicleNames;

        static VehicleConvert()
        {
            vehicleNames = new Dictionary<DomainVehicleHash, string>();
        }

        public static string GetVehicleName(DomainVehicleHash vehicleHash)
        {
            string vehicleName;
            if (vehicleNames.TryGetValue(vehicleHash, out vehicleName))
            {
                return vehicleName;
            }

            return vehicleHash.ToString();
        }
    }
}