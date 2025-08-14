using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Interfaces;
using Concesionario.Domain.Entities.Vehicles;
using Concesionario.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Services.Vehicles
{
   public class VehicleManagementService : IVehicleService
    {
        private readonly IRepository _repository;
        public VehicleManagementService(IRepository repository) {
        
            _repository = repository;
        }


        public async Task<IEnumerable<VehicleResponseDto>?> GetVehicles() {

            return (await _repository.GetAll<Vehicle>())?.Select(vehicle => new VehicleResponseDto(vehicle.LicensePlate, vehicle.Year, vehicle.Color, vehicle.Version, vehicle.Mileage, vehicle.Description, vehicle.Transmission, vehicle.FuelType, vehicle.VehicleType, vehicle.Status)); 


        }
    }
}
