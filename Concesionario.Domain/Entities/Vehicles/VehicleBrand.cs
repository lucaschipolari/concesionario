using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Vehicles
{
    public class VehicleBrand : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public ICollection<VehicleModel> Models { get; set; } = new List<VehicleModel>();
    }
}
