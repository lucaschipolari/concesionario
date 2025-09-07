using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.SalesReservation
{
    public class TaxCalculator
    {
        private readonly decimal? _ivaRate;
        private readonly decimal? _stampTaxRate;

        public TaxCalculator(decimal ivaRate, decimal stampTaxRate)
        {
            _ivaRate = ivaRate;
            _stampTaxRate = stampTaxRate;
        }

        public (decimal iva, decimal stamp, decimal total) Calculate(decimal basePrice)
        {
            if (basePrice <= 0)
                throw new ArgumentException("El precio base debe ser mayor que cero");

            var iva = basePrice * (_ivaRate ?? 0m);
            var stamp = basePrice * (_stampTaxRate ?? 0m);
            var total = basePrice + iva + stamp;
            return (iva, stamp, total);
        }

    }

}
