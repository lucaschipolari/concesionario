using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}
