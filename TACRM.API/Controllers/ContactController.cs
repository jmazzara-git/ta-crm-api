using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Business.Abstractions;
using TACRM.Services.Dtos;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContactController(IContactService contactsService) : ControllerBase
	{
		private readonly IContactService _contactsService = contactsService;

		// GET: api/contacts
		[HttpGet]
		public async Task<IActionResult> GetContacts()
		{
			return Ok(await _contactsService.GetListAsync());
		}

		// GET: api/contact/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetContact(int id)
		{
			return Ok(await _contactsService.GetByIdAsync(id));
		}

		// POST: api/contact
		[HttpPost]
		public async Task<IActionResult> CreateContact(ContactDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _contactsService.CreateAsync(dto));
		}

		// PUT: api/contact
		[HttpPut]
		public async Task<IActionResult> UpdateContact(ContactDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _contactsService.UpdateAsync(dto));
		}

		// DELETE: api/contact/[id]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			return Ok(await _contactsService.DeleteAsync(id));
		}

		// POST: api/contact/search
		[HttpPost("search")]
		public async Task<IActionResult> SearchContacts(ContactSearchRequestDto dto)
		{
			if (dto == null)
				return BadRequest();

			return Ok(await _contactsService.SearchAsync(dto));
		}

		// GET: api/contact/sources
		[HttpGet("sources")]
		public async Task<IActionResult> GetContactSources()
		{
			return Ok(await _contactsService.GetContactSourcesAsync());
		}

		// GET: api/contact/statuses
		[HttpGet("statuses")]
		public async Task<IActionResult> GetContactStatuses()
		{
			return Ok(await _contactsService.GetContactStatusesAsync());
		}
	}
}