using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContactsController : ControllerBase
	{
		private readonly IContactService _contactService;
		private readonly ILogger<ContactsController> _logger;

		public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
		{
			_contactService = contactService;
			_logger = logger;
		}

		// GET: api/contacts
		[HttpGet]
		public async Task<IActionResult> GetAllContacts()
		{
			var contacts = await _contactService.GetAllContactsAsync();
			return Ok(contacts);
		}

		// GET: api/contacts/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetContactById(int id)
		{
			var contact = await _contactService.GetContactByIdAsync(id);
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

			var createdContact = await _contactService.CreateContactAsync(contact);
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

			var updatedContact = await _contactService.UpdateContactAsync(contact);
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
			var deleted = await _contactService.DeleteContactAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return NoContent();
		}
	}
}
