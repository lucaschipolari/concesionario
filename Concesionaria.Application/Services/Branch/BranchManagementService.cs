using Concesionario.Application.Dto.Branch;
using Concesionario.Application.Interfaces;
using Concesionario.Domain.Entities.Branches;
using BranchEntity = Concesionario.Domain.Entities.Branches.Branch;
using Concesionario.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Services.Branch
{
    public class BranchManagementService : IBranchService
    {
        private readonly IRepository _repository;
        public BranchManagementService(IRepository repository)
        {

            _repository = repository;
        }
        public async Task<BranchResponseDto?> AddBranch(BranchRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Branch request cannot be null");
            }
            var branch = new BranchEntity
            {
                Location = request.Location
            };
            await _repository.Add<BranchEntity>(branch);
            return new BranchResponseDto(branch.Id, branch.Location);
        }
        public async Task<bool> DeleteBranch(Guid branchId)
        {
            var branch = await _repository.GetById<BranchEntity>(branchId);
            if (branch == null)
            {
                return false;
            }
            await _repository.Delete<BranchEntity>(branch);
            return true;
        }
        public async Task<IEnumerable<BranchResponseDto>> GetBranches()
        {
            var branches = await _repository.GetAll<BranchEntity>();
            return branches.Select(b => new BranchResponseDto(b.Id, b.Location));
        }
        
    }
}
