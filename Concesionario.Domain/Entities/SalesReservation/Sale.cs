using Concesionario.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.SalesReservation
{
    public class Sale : EntityBase
    {
        public Customer? Customer { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid ReservationId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; }
    }
}
