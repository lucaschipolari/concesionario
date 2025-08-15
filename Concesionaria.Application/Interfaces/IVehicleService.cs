using Concesionario.Application.Dto.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleResponseDto>?> GetVehicles();

        Task<VehicleResponseDto> AddVehicle(VehicleRequestDto vehicleRequestDto);
    }
}
