using Concesionario.Application.Dto.Branch;
using Concesionario.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService) {

            _branchService = branchService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch([FromBody] BranchRequestDto request)
        {
            try
            {
                var branch = await _branchService.AddBranch(request);
                return Ok(branch);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            try
            {
                var branches = await _branchService.GetBranches();
                return Ok(branches);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
    }
}
