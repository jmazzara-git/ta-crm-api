using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SaleProductsController : ControllerBase
	{
		private readonly ISaleProductsService _saleProductsService;

		public SaleProductsController(ISaleProductsService saleProductsService)
		{
			_saleProductsService = saleProductsService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllSaleProducts()
		{
			var saleProducts = await _saleProductsService.GetAllSaleProductsAsync();
			return Ok(saleProducts);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetSaleProductById(int id)
		{
			var saleProduct = await _saleProductsService.GetSaleProductByIdAsync(id);
			if (saleProduct == null) return NotFound();

			return Ok(saleProduct);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSaleProduct([FromBody] SaleProduct saleProduct)
		{
			var createdSaleProduct = await _saleProductsService.CreateSaleProductAsync(saleProduct);
			return CreatedAtAction(nameof(GetSaleProductById), new { id = createdSaleProduct.SaleProductID }, createdSaleProduct);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateSaleProduct(int id, [FromBody] SaleProduct saleProduct)
		{
			if (id != saleProduct.SaleProductID) return BadRequest();

			var updatedSaleProduct = await _saleProductsService.UpdateSaleProductAsync(saleProduct);
			if (updatedSaleProduct == null) return NotFound();

			return Ok(updatedSaleProduct);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSaleProduct(int id)
		{
			var success = await _saleProductsService.DeleteSaleProductAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
