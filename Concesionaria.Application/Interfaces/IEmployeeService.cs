using Concesionario.Application.Dto.Core.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>> GetEmployees();

        Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByBranchId(Guid id);
        Task<bool> DeleteEmployee(Guid employeeId);
    }
}
