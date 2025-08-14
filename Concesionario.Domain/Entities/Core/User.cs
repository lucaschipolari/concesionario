using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Concesionario.Domain.Entities.Core
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string DNI { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;

        // Relaciones
        public Employee? Employee { get; set; }
        public Customer? Customer { get; set; }
    }


}
