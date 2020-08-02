using Shared.CrossCuting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(vehicleNames.TryGetValue(vehicleHash, out vehicleName))
            {
                return vehicleName;
            }

            return vehicleHash.ToString();
        }

    }
}
