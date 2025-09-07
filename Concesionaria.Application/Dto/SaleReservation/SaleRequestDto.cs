using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Dto.SaleReservation
{
    public record SaleRequestDto(
        Guid CustomerId,
        Guid VehicleId

        );
}
