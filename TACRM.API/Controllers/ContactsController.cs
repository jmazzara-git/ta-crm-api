using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContactsController(IContactsService contactService, ILogger<ContactsController> logger) : ControllerBase
	{
		private readonly IContactsService _contactsService = contactService;
		private readonly ILogger<ContactsController> _logger = logger;

		// GET: api/contacts
		[HttpGet]
		public async Task<IActionResult> GetAllContacts()
		{
			var contacts = await _contactsService.GetAllContactsAsync();
			return Ok(contacts);
		}

		// GET: api/contacts/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetContactById(int id)
		{
			var contact = await _contactsService.GetContactByIdAsync(id);
			if (contact == null)
			{
				return NotFound();
			}
			return Ok(contact);
		}

		// POST: api/contacts
		[HttpPost]
		public async Task<IActionResult> CreateContact([FromBody] Contact contact)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var createdContact = await _contactsService.CreateContactAsync(contact);
			return CreatedAtAction(nameof(GetContactById), new { id = createdContact.ContactID }, createdContact);
		}

		// PUT: api/contacts/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact contact)
		{
			if (id != contact.ContactID || !ModelState.IsValid)
			{
				return BadRequest();
			}

			var updatedContact = await _contactsService.UpdateContactAsync(contact);
			if (updatedContact == null)
			{
				return NotFound();
			}

			return Ok(updatedContact);
		}

		// DELETE: api/contacts/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			var deleted = await _contactsService.DeleteContactAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
