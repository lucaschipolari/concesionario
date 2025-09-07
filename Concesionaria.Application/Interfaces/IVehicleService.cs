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

        Task<IEnumerable<VehicleResponseDto>?> GetAvalibleVehicles();
        Task<VehicleResponseDto> AddVehicle(VehicleRequestDto vehicleRequestDto);

        Task<VehicleResponseDto> UpdateVehicle(Guid id, VehicleRequestDto vehicleRequestDto);

        Task DeleteVehicle(Guid id);

        Task<VehicleModelResponseDto?> AddVehicleModel(VehicleModelRequestDto request);
        Task<IEnumerable<VehicleModelResponseDto>?> GetVehicleModels();

        Task <VehicleBrandResponseDto> AddVehicleBrand(VehicleBrandRequestDto vehicleBrandRequestDto);

        Task <IEnumerable<VehicleBrandResponseDto>> GetBrands();
    }
}
