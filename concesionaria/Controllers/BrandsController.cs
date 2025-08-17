using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public BrandsController(IVehicleService vehicleService) {
        
            _vehicleService = vehicleService;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            try {
                var brands = await _vehicleService.GetBrands();
                return Ok(brands);
            }
            catch(Exception e) {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddBrand([FromBody] VehicleBrandRequestDto request) {
            try {
                var brand = await _vehicleService.AddVehicleBrand(request);
                return Ok(brand);
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }
    }
}
