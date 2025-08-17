using Azure.Core;
using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Exceptions;
using Concesionario.Application.Interfaces;
using Concesionario.Domain.Entities.Vehicles;
using Concesionario.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

            return (await _repository.GetAll<Vehicle>())?.Select(vehicle => new VehicleResponseDto(vehicle.Id,vehicle.LicensePlate, vehicle.Year, vehicle.Color, vehicle.Version, vehicle.Mileage, vehicle.Description, vehicle.Transmission, vehicle.FuelType, vehicle.VehicleType, vehicle.Status)); 


        }
        public async Task<VehicleResponseDto?> UpdateVehicle(Guid id,VehicleRequestDto vehicleRequestDto) {
            if (string.IsNullOrWhiteSpace(vehicleRequestDto.LicensePlate) ||
             string.IsNullOrWhiteSpace(vehicleRequestDto.Color) ||
          vehicleRequestDto.Mileage < 0 || !Enum.IsDefined(typeof(VehicleStatus), vehicleRequestDto.Status)
          || !Enum.IsDefined(typeof(Transmission), vehicleRequestDto.Transmission)
          || !Enum.IsDefined(typeof(VehicleType), vehicleRequestDto.VehicleType))
            {
                throw new ArgumentException("Valores para el producto no válidos");
            }

            var existingVehicle = await _repository.GetById<Vehicle>(id);
            if (existingVehicle == null) {
                throw new EntityNotFoundException("Entidad no encontrada");
            }
            var vehicle = await _repository.GetFiltered<Vehicle>(v => v.LicensePlate == vehicleRequestDto.LicensePlate);
            if (vehicle != null)
            {
                throw new ArgumentException("Ya existe un vehiculo con esa matricula");
            }
            existingVehicle.Color= vehicleRequestDto.Color;
            existingVehicle.Version = vehicleRequestDto.Version;
            existingVehicle.Status = vehicleRequestDto.Status;
            existingVehicle.ModelId = vehicleRequestDto.ModelId;
            existingVehicle.FuelType = vehicleRequestDto.FuelType;
            existingVehicle.Description = vehicleRequestDto.Description;
            existingVehicle.Mileage = vehicleRequestDto.Mileage;
            existingVehicle.Year = vehicleRequestDto.Year;
            existingVehicle.Transmission = vehicleRequestDto.Transmission;

            await _repository.Update<Vehicle>(existingVehicle);

            return new VehicleResponseDto(existingVehicle.Id,existingVehicle.LicensePlate, existingVehicle.Year, existingVehicle.Color, existingVehicle.Version, existingVehicle.Mileage, existingVehicle.Description, existingVehicle.Transmission, existingVehicle.FuelType, existingVehicle.VehicleType, existingVehicle.Status);

        }
        public async Task<VehicleResponseDto> AddVehicle(VehicleRequestDto vehicleRequestDto) {

            if (string.IsNullOrWhiteSpace(vehicleRequestDto.LicensePlate) ||
               string.IsNullOrWhiteSpace(vehicleRequestDto.Color) ||
            vehicleRequestDto.Mileage < 0 || !Enum.IsDefined(typeof(VehicleStatus), vehicleRequestDto.Status) 
            || !Enum.IsDefined(typeof(Transmission), vehicleRequestDto.Transmission) 
            || !Enum.IsDefined(typeof(VehicleType) ,vehicleRequestDto.VehicleType))
                {
                    throw new ArgumentException("Valores para el producto no válidos");
                }
    

            var exist = await _repository.First<Vehicle>(v => v.LicensePlate == vehicleRequestDto.LicensePlate);
            if (exist != null) throw new DuplicatedEntityException("Ya existe un vehiculo");
            var model = await _repository.GetById<VehicleModel>(vehicleRequestDto.ModelId);
            if (model == null) throw new EntityNotFoundException("Modelo de vehiculo no encontrado");
            var vehicle= new Vehicle(
     vehicleRequestDto?.LicensePlate,
     vehicleRequestDto?.Year ?? 0,
     vehicleRequestDto?.Color,
     vehicleRequestDto?.Version,
     vehicleRequestDto?.Mileage,
     vehicleRequestDto?.Description,
     vehicleRequestDto.Transmission,
     vehicleRequestDto.FuelType,
     vehicleRequestDto.VehicleType,
     vehicleRequestDto.Status,
     vehicleRequestDto.ModelId
 );
            await _repository.Add(vehicle);

            return new VehicleResponseDto(vehicle.Id,vehicle.LicensePlate,vehicle.Year,vehicle.Color,vehicle.Version,vehicle.Mileage,vehicle.Description,vehicle.Transmission,vehicle.FuelType,vehicle.VehicleType,vehicle.Status);
        }
        public async Task<VehicleModelResponseDto?> AddVehicleModel(VehicleModelRequestDto request) {

            if (string.IsNullOrWhiteSpace(request.Name)) {
                throw new ArgumentException("Datos ingresados no validos");
            }

            var existingBrand = await _repository.GetById<VehicleBrand>(request.BrandId);
            if (existingBrand == null) throw new EntityNotFoundException("No existe la marca de auto");

            var vehicleModel = new VehicleModel(request.Name,request.BrandId);

            await _repository.Add(vehicleModel);

            return new VehicleModelResponseDto(vehicleModel.Name,vehicleModel.Id);



        }
        public async Task<IEnumerable<VehicleModelResponseDto>?> GetVehicleModels()
        {

            return (await _repository.GetAll<VehicleModel>())?.Select(m => new VehicleModelResponseDto(m.Name,m.Id));


        }
        public async Task<VehicleBrandResponseDto?> AddVehicleBrand(VehicleBrandRequestDto request)
        {

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Country))
            {
                throw new ArgumentException("Datos ingresados no validos");
            }
            
            var existingBrand = await _repository.GetFiltered<VehicleBrand>(b => b.Name.ToLower() == request.Name.ToLower());
            if (existingBrand is { } brands && brands.Any())
                throw new DuplicatedEntityException("Ya existe una marca con dicho nombre");

            var brand = new VehicleBrand(request.Name, request.Country);

            await _repository.Add(brand);

            return new VehicleBrandResponseDto(brand.Name, brand.Country,brand.Id);



        }
        public async Task<IEnumerable<VehicleBrandResponseDto>?> GetBrands()
        {
            return (await _repository.GetAll<VehicleBrand>())?.Select(m => new VehicleBrandResponseDto(m.Name,m.Country, m.Id));
        }
    }
}
