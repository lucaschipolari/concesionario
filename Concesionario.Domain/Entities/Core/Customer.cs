using Concesionario.Domain.Entities.SalesReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Core
{
    public class Customer : EntityBase
    {
        public Guid UserId { get; set; }  
        public User User { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
