using Concesionario.Application.Dto.Core;
using Concesionario.Application.Dto.SaleReservation;
using Concesionario.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Concesionario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }
        [HttpPost]
        public async Task<IActionResult> AddSale([FromBody] SaleRequestDto request)
        {
            try
            {
                var sale = await _saleService.ProcessSale(request);
                return Ok(sale);
             
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("/reservas/{id}")]
        public async Task<IActionResult> AddSaleReservation([FromRoute] Guid id, [FromBody] SaleRequestDto request)
        {
            try
            {
                var sale = await _saleService.ProcessSale(request, id);
                return Ok(sale);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            try
            {
                // Aquí iría la lógica para obtener las ventas utilizando un servicio, por ejemplo:
                var sales = await _saleService.GetSales();
                return Ok(sales);
                // Por ahora, solo devolvemos una lista vacía para demostrar que funciona.
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
