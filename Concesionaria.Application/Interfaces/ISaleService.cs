using Concesionario.Application.Dto.SaleReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface ISaleService
    {
        Task<SaleResponseDto> ProcessSale(SaleRequestDto saleRequestDto, Guid? saleReservationId = null);

        Task<SaleResponseDto> ReserveVehicle(Guid vehicleId, Guid userId);

        Task<IEnumerable<SaleResponseDto>?> GetSales();



    }
}
