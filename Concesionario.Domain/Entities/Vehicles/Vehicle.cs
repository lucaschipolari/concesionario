using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Vehicles
{
    public class Vehicle : EntityBase
    {
        public Vehicle( decimal? price, string? licensePlate, int year, string? color, string? version, double? mileage,
            string? description, Transmission transmission, FuelType fuelType, VehicleType vehicleType,
            VehicleStatus status, Guid modelId)
        {
            LicensePlate = licensePlate;
            Year = year;
            Color = color;
            Version = version;
            Mileage = mileage;
            Description = description;
            Transmission = transmission;
            FuelType = fuelType;
            VehicleType = vehicleType;
            Status = status;
            ModelId = modelId;
            IsActive = true;
            Price = price;
        }

        public string? LicensePlate { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public string? Version { get; set; }
        public double? Mileage { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }

        public Transmission Transmission { get; set; }
        public FuelType FuelType { get; set; }
        public VehicleType VehicleType { get; set; }
        public VehicleStatus Status { get; set; }

        public bool IsActive { get; set; }
        public Guid ModelId { get; set; }
        public VehicleModel Model { get; set; } = null!;
    }
}
