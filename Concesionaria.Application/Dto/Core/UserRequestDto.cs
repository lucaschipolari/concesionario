using Concesionario.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Dto.Core
{
    public record UserRequestDto(
    string FullName,
    string DNI,
    string Phone,
    bool IsActive,
    string Email,
    string Password,
    UserType UserType
);
}
