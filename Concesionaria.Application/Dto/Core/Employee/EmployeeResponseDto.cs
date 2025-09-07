using Concesionario.Domain.Entities.Core;

namespace Concesionario.Application.Dto.Core.Employee
{
    public record EmployeeResponseDto(
        string FullName,
    string DNI,
    string Phone,
    bool IsActive,
    string Email,
    UserType UserType,
    Guid? BranchId = null,
    Guid? PositionId = null
        );
}