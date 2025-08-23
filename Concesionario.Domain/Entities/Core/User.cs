using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Concesionario.Domain.Entities.Core
{
    public class User : EntityBase
    {

        public string FullName { get; set; }
        public string DNI { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public UserType UserType { get; set; }

        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
      
    }


}
