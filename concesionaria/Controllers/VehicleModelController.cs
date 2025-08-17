using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelController(IVehicleService vehicleService) {

            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleModels() {
            try
            {
                var models = await _vehicleService.GetVehicleModels();
                return Ok(models);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddVehicleModel([FromBody] VehicleModelRequestDto request) {
            try {
                var result = await _vehicleService.AddVehicleModel(request);
                return Ok(result);

            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        
    }
}
