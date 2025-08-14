using Concesionario.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.SalesReservation
{
    public class Reservation :EntityBase
    {
        public Guid VehicleId { get; set; }   
        
        public Customer? Customer { get; set; }
        public Guid CustomerId { get; set; }         
        public DateTime StartDate { get; set; }     
        public DateTime EndDate { get; set; }        
        public decimal ReservationAmount { get; set; } 
        public ReservationStatus Status { get; set; }

    }
}
