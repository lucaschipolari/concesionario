using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Task <Guid?> GetCurrentUserId();
    }
}
