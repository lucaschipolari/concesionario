using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Finance
{
    public class Installment :EntityBase
    {
        public Guid FinancingPlanId { get; set; }
        public int Number { get; set; }               
        public decimal Amount { get; set; }           
        public DateTime DueDate { get; set; }         
        public InstallmentStatus Status { get; set; } 
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
