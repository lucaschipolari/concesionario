using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Finance
{
    public class FinancingPlan :EntityBase
    {
        public Guid SaleId { get; set; }               
        public decimal TotalAmount { get; set; }     
        public int NumberOfInstallments { get; set; } 
        public decimal InterestRate { get; set; }     
        public DateTime StartDate { get; set; }       

        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
    }
}
