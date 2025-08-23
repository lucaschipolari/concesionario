using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Concesionario.Domain.Entities.Vehicles;


namespace Concesionario.Application.Dto.Vehicles
{
    public record VehicleRequestDto(double Price,double PromotionalPrice,string LicensePlate, int Year, string Color, string Version, double Mileage, string Description, Transmission Transmission, FuelType FuelType, VehicleType VehicleType, VehicleStatus Status,Guid ModelId);
}
