using Concesionario.Application.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> AddUser(UserRequestDto request);

    }
}
