using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProvidersController : ControllerBase
	{
		private readonly IProvidersService _providersService;

		public ProvidersController(IProvidersService providerService)
		{
			_providersService = providerService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProviders()
		{
			var providers = await _providersService.GetAllProvidersAsync();
			return Ok(providers);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetProviderById(int id)
		{
			var provider = await _providersService.GetProviderByIdAsync(id);
			if (provider == null) return NotFound();

			return Ok(provider);
		}

		[HttpPost]
		public async Task<IActionResult> CreateProvider([FromBody] Provider provider)
		{
			var createdProvider = await _providersService.CreateProviderAsync(provider);
			return CreatedAtAction(nameof(GetProviderById), new { id = createdProvider.ProviderID }, createdProvider);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProvider(int id, [FromBody] Provider provider)
		{
			if (id != provider.ProviderID) return BadRequest();

			try
			{
				var updatedProvider = await _providersService.UpdateProviderAsync(provider);
				return Ok(updatedProvider);
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProvider(int id)
		{
			var success = await _providersService.DeleteProviderAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
