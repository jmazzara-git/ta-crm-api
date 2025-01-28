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

		[HttpGet]
		public async Task<IActionResult> GetAllContacts()
		{
			var contacts = await _contactsService.GetAllContactsAsync();
			return Ok(contacts);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetContactById(int id)
		{
			var contact = await _contactsService.GetContactByIdAsync(id);
			if (contact == null)
				return NotFound();

			return Ok(contact);
		}

		[HttpPost]
		public async Task<IActionResult> CreateContact(Contact contact)
		{
			try
			{
				var createdContact = await _contactsService.CreateContactAsync(contact);
				return CreatedAtAction(nameof(GetContactById), new { id = createdContact.ContactID }, createdContact);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateContact(int id, Contact contact)
		{
			if (id != contact.ContactID)
				return BadRequest("Contact ID mismatch.");

			try
			{
				var updatedContact = await _contactsService.UpdateContactAsync(contact);
				if (updatedContact == null)
					return NotFound();

				return Ok(updatedContact);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			var result = await _contactsService.DeleteContactAsync(id);
			if (!result)
				return NotFound();

			return NoContent();
		}

		[HttpPost("{contactId}/products/{productId}")]
		public async Task<IActionResult> AddProductInterest(int contactId, int productId)
		{
			//await _contactsService.AddProductInterestAsync(contactId, productId);
			return NoContent();
		}

		[HttpDelete("products/{id}")]
		public async Task<IActionResult> RemoveProductInterest(int id)
		{
			//await _contactsService.RemoveProductInterestAsync(id);
			return NoContent();
		}
	}
}
