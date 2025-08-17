using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Dto.Vehicles
{
    public record VehicleModelResponseDto(string Name, Guid ModelId);
}
