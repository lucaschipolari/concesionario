using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Concesionario.Domain.Entities.Core;

namespace Concesionario.Data.Identity
{
    public class ApplicationUserIdentity : IdentityUser<Guid>
    {
        public Guid DomainUserId { get; set; }
        public User DomainUser { get; set; }

        public string FullName { get; set; }
        public string DNI { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;

        public UserType UserType { get; set; }
    }
}