using Azure.Core;
using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Exceptions;
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

        public async Task<VehicleResponseDto> AddVehicle(VehicleRequestDto vehicleRequestDto) {

            if (string.IsNullOrWhiteSpace(vehicleRequestDto.LicensePlate) ||
               string.IsNullOrWhiteSpace(vehicleRequestDto.Color) ||
            vehicleRequestDto.Mileage < 0 || !Enum.IsDefined(typeof(VehicleStatus), vehicleRequestDto.Status) || !Enum.IsDefined(typeof(Transmission), vehicleRequestDto.Transmission) || !Enum.IsDefined(typeof(VehicleType) ,vehicleRequestDto.VehicleType))
                {
                    throw new ArgumentException("Valores para el producto no válidos");
                }

            var exist = await _repository.First<Vehicle>(v => v.LicensePlate == vehicleRequestDto.LicensePlate);
            if (exist != null) throw new DuplicatedEntityException("Ya existe un vehiculo");

            var vehiculo = new Vehicle(
     vehicleRequestDto?.LicensePlate,
     vehicleRequestDto?.Year ?? 0,
     vehicleRequestDto?.Color,
     vehicleRequestDto?.Version,
     vehicleRequestDto?.Mileage,
     vehicleRequestDto?.Description,
     vehicleRequestDto?.Transmission ?? Transmission.Manual,
     vehicleRequestDto?.FuelType ?? FuelType.Petrol,
     vehicleRequestDto?.VehicleType ?? VehicleType.Car, // ← Este es el que faltaba
     vehicleRequestDto?.Status ?? VehicleStatus.Available,
     vehicleRequestDto?.ModelId ?? Guid.NewGuid()
 );

            return new VehicleResponseDto();
        }
    }
}
