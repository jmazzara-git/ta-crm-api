using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Dtos;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController(IGenericService<ProductDto> productService) : ControllerBase
	{
		private readonly IGenericService<ProductDto> _productService = productService;

		// GET: api/products
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			return Ok(await _productService.GetListAsync());
		}

		// GET: api/product/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProduct(int id)
		{
			return Ok(await _productService.GetByIdAsync(id));
		}

		// POST: api/product
		[HttpPost]
		public async Task<IActionResult> CreateProduct(ProductDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _productService.CreateAsync(dto));
		}

		// PUT: api/product
		[HttpPut]
		public async Task<IActionResult> UpdateProduct(ProductDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _productService.UpdateAsync(dto));
		}

		// DELETE: api/product/[id]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			return Ok(await _productService.DeleteAsync(id));
		}
	}
}