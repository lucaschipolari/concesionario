using Concesionario.Application.Dto.SaleReservation;
using Concesionario.Application.Interfaces;
using Concesionario.Domain.Entities;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.SalesReservation;
using Concesionario.Domain.Entities.Vehicles;
using Concesionario.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Services.SalesReservation
{
    public class SaleManagementService : ISaleService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository _repository;
        private readonly TaxCalculator _taxCalculator;
        
        public SaleManagementService(TaxCalculator taxCalculator,ICurrentUserService currentUserService, IRepository repository)
        {
            _currentUserService = currentUserService;
            _repository = repository;
            _taxCalculator = taxCalculator;
        }

        public async Task<SaleResponseDto> ProcessSale(SaleRequestDto saleRequestDto,Guid? saleReservationId = null)
        {
            var userId = await _currentUserService.GetCurrentUserId();

            if (userId.Value == Guid.Empty)
            {
                throw new UnauthorizedAccessException("El usuario no está autenticado.");
            }
            if (saleRequestDto.CustomerId == Guid.Empty  || saleRequestDto.VehicleId == Guid.Empty)
            {
                throw new ArgumentException("Datos de la solicitud de venta incompletos.");
            }

      
            var employee = await _repository.First<Employee>(e => e.UserId == userId.Value,nameof(Employee.Branch));
            if (employee == null)
            {
                throw new UnauthorizedAccessException("El usuario no está registrado como empleado.");
            }

            var customer = await _repository.First<Customer>(c => c.UserId == saleRequestDto.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException("El cliente especificado no es válido.");
            }

            var vehicle = await _repository.GetById<Vehicle>(saleRequestDto.VehicleId);
            if (vehicle == null || vehicle.Status != VehicleStatus.Available)
            {
                throw new ArgumentException("El vehículo especificado no está disponible para la venta.");
            }
            var existingSale = await _repository.GetFiltered<Sale>(s => s.VehicleId == saleRequestDto.VehicleId && s.Status == SaleStatus.Paid);
            if (existingSale.Any())
            {
                throw new InvalidOperationException("El vehículo ya ha sido vendido.");
            }
            if (saleReservationId != null && saleReservationId != Guid.Empty)
            {
                var reservation = await _repository.GetById<Reservation>(saleReservationId.Value);
                if (reservation == null || reservation.Status != ReservationStatus.Pending || reservation.VehicleId != saleRequestDto.VehicleId || reservation.CustomerId != saleRequestDto.CustomerId)
                {
                    throw new ArgumentException("La reserva especificada no es válida para esta venta.");
                }
                reservation.Status = ReservationStatus.Confirmed;
                await _repository.Update<Reservation>(reservation);
            }


            if (vehicle.Price == null)
            {
                throw new InvalidOperationException($"El vehículo no tiene precio definido.");
            }

            var (iva, stamp, total) = _taxCalculator.Calculate(vehicle.Price.Value);

            var sale = new Sale
            {
                CustomerId = customer.Id,
                VehicleId = saleRequestDto.VehicleId,
                SaleDate = DateTime.UtcNow,
                FinalPrice =total,
                IVA=iva,
                BasePrice =vehicle.Price,
                OtherTaxes=stamp,
                EmployeeId = employee.Id,
                Status = SaleStatus.Paid,
                ReservationId = saleReservationId?? null
            };

            await _repository.Add<Sale>(sale);
            
            vehicle.Status = VehicleStatus.Sold;
            await _repository.Update<Vehicle>(vehicle);
            return new SaleResponseDto(
                sale.SaleDate,
                total,
                iva,
                stamp,
                userId.Value,
                saleRequestDto.CustomerId,
                saleRequestDto.VehicleId,
                saleReservationId?? null
                );
        }


        public async Task<IEnumerable<SaleResponseDto>?> GetSales() {
            return (await _repository.GetAll<Sale>())
    ?.Where(s => s.FinalPrice.HasValue && s.IVA.HasValue && s.OtherTaxes.HasValue)
    .Select(s => new SaleResponseDto(
        s.SaleDate,
        s.FinalPrice.Value,
        s.IVA.Value,
        s.OtherTaxes.Value,
        s.EmployeeId ?? Guid.Empty,
    s.CustomerId ?? Guid.Empty,
    s.VehicleId ?? Guid.Empty,
        s.ReservationId ?? Guid.Empty
    ));
        }
        public Task<SaleResponseDto> ReserveVehicle(Guid vehicleId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
