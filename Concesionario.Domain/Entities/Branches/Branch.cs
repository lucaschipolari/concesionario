using Concesionario.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Branches
{
    public class Branch : EntityBase
    {
        public string Location { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
