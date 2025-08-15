using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService) { 
        
            _vehicleService = vehicleService;
        }
        // GET: api/<VehicleController>
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetVehicles();
                if (vehicles == null || !vehicles.Any()) {
                    return NoContent();
                }
                return Ok(vehicles);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);   
            }
        }

        // GET api/<VehicleController>/5
        // [HttpGet("{id}")]
        //public async Task<IActionResult> GetVehicleById(int id)
        //{
        //    return ;
        //}

        //// POST api/<VehicleController>
        [HttpPost]
        public async Task<IActionResult> PostVehicle([FromBody] VehicleRequestDto value)
        {
            try
            {
               var result = await _vehicleService.AddVehicle(value);
               return Ok(result);
            }
            catch (Exception ex) { 

                return BadRequest(ex.Message);

            }

        }

        // PUT api/<VehicleController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] string value)
        {

        }

        // DELETE api/<VehicleController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
