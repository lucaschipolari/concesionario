using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Dto.SaleReservation
{
    public record SaleResponseDto(
        DateTime Date,
        decimal FinalPrice,
        decimal iva,
        decimal stamp,
        Guid UserId,
        Guid ClientId,
        Guid VehicleId,
        Guid? SaleReservationId = null
        );
}
