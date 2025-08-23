using Concesionario.Domain.Entities.Branches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Domain.Entities.Core
{
    public class Employee : EntityBase
    {
        public Guid UserId { get; set; }  
        public User User { get; set; }

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }

        public Guid PositionId { get; set; }
        public Position Position { get; set; }
    }



}
