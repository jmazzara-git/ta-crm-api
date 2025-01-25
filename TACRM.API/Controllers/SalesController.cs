using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class SalesController : ControllerBase
	{
		private readonly ISalesService _salesService;

		public SalesController(ISalesService salesService)
		{
			_salesService = salesService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllSales()
		{
			var sales = await _salesService.GetAllSalesAsync();
			return Ok(sales);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetSaleById(int id)
		{
			var sale = await _salesService.GetSaleByIdAsync(id);
			if (sale == null) return NotFound();

			return Ok(sale);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSale([FromBody] Sale sale)
		{
			var createdSale = await _salesService.CreateSaleAsync(sale);
			return CreatedAtAction(nameof(GetSaleById), new { id = createdSale.SaleID }, createdSale);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateSale(int id, [FromBody] Sale sale)
		{
			if (id != sale.SaleID) return BadRequest();

			try
			{
				var updatedSale = await _salesService.UpdateSaleAsync(sale);
				return Ok(updatedSale);
			}
			catch (UnauthorizedAccessException)
			{
				return Forbid();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSale(int id)
		{
			var success = await _salesService.DeleteSaleAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
