using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Vehicles
{
    public class VehicleModel : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public Guid BrandId { get; set; }

        public VehicleBrand Brand { get; set; } = null!;
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }


}
