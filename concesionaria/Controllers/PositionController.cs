using Concesionario.Application.Dto.Core.Employee;
using Concesionario.Application.Interfaces;
using Concesionario.Domain.Entities.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        public PositionController(IPositionService positionService) { 
        
            _positionService = positionService;
        }
        [HttpPost]
        public async Task<IActionResult> AddPosition([FromBody] PositionRequestDto request)
        {
            try
            {
                var position = await _positionService.AddPosition(request);
                return Ok(position);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPositions()
        {
            try
            {
                var positions = await _positionService.GetPositions();
                return Ok(positions);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}
