using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.Vehicles;
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
        public Vehicle? Vehicle { get; set; }
        public Guid? VehicleId { get; set; }
        public Employee? Employee { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? IVA { get; set; }
        public decimal? OtherTaxes { get; set; }
        public decimal? FinalPrice { get; set; }
        public SaleStatus Status { get; set; }
    }
}
