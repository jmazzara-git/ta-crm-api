using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Dtos;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProviderController(IGenericService<ProviderDto> providerService) : ControllerBase
	{
		private readonly IGenericService<ProviderDto> _providerService = providerService;

		// GET: api/providers
		[HttpGet]
		public async Task<IActionResult> GetProviders()
		{
			return Ok(await _providerService.GetListAsync());
		}

		// GET: api/provider/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProvider(int id)
		{
			return Ok(await _providerService.GetByIdAsync(id));
		}

		// POST: api/provider
		[HttpPost]
		public async Task<IActionResult> CreateProvider(ProviderDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _providerService.CreateAsync(dto));
		}

		// PUT: api/provider
		[HttpPut]
		public async Task<IActionResult> UpdateProvider(ProviderDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _providerService.UpdateAsync(dto));
		}

		// DELETE: api/provider/[id]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProvider(int id)
		{
			return Ok(await _providerService.DeleteAsync(id));
		}
	}
}