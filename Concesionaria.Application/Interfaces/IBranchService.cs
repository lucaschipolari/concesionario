using Concesionario.Application.Dto.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchResponseDto>> GetBranches();
        Task<BranchResponseDto> AddBranch(BranchRequestDto branchRequest);

        Task<bool> DeleteBranch(Guid branchId);
    }
}
