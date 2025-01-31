using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Abstractions;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContactsController : ControllerBase
	{
		private readonly IContactsService _contactsService;

		public ContactsController(IContactsService contactsService)
		{
			_contactsService = contactsService;
		}

		// GET: api/contacts
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
		{
			var contacts = await _contactsService.GetContactsAsync();
			return Ok(contacts);
		}

		// GET: api/contacts/search
		[HttpGet("search")]
		public async Task<IActionResult> SearchContacts(
			[FromQuery] string searchTerm = null,
			[FromQuery] int? contactStatusId = null,
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
		{
			var (contacts, totalCount) = await _contactsService.SearchContactsAsync(
				searchTerm,
				contactStatusId,
				pageNumber,
				pageSize);

			return Ok(new
			{
				Data = contacts,
				TotalCount = totalCount,
				PageNumber = pageNumber,
				PageSize = pageSize
			});
		}

		// GET: api/contacts/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Contact>> GetContact(int id)
		{
			var contact = await _contactsService.GetContactByIdAsync(id);

			if (contact == null)
			{
				return NotFound("Contact not found");
			}

			return Ok(contact);
		}

		// POST: api/contacts
		[HttpPost]
		public async Task<ActionResult<Contact>> CreateContact(Contact contact)
		{
			try
			{
				var createdContact = await _contactsService.CreateContactAsync(contact);
				return CreatedAtAction(nameof(GetContact), new { id = createdContact.ContactID }, createdContact);
			}
			catch (ValidationException ex)
			{
				return BadRequest(new
				{
					Message = "Validation failed",
					Errors = ProcessValidationErrors(ex.Errors)
				});
			}
		}

		// PUT: api/contacts/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateContact(int id, Contact contact)
		{
			try
			{
				if (id != contact.ContactID)
				{
					return BadRequest("Contact ID mismatch");
				}

				await _contactsService.UpdateContactAsync(contact);
				return NoContent();
			}
			catch (ValidationException ex)
			{
				return BadRequest(new
				{
					Message = "Validation failed",
					Errors = ProcessValidationErrors(ex.Errors)
				});
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Contact not found");
			}
		}

		// DELETE: api/contacts/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			try
			{
				await _contactsService.DeleteContactAsync(id);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Contact not found");
			}
		}

		private List<string> ProcessValidationErrors(IEnumerable<ValidationFailure> errors)
		{
			var errorMessages = new List<string>();
			foreach (var error in errors)
			{
				errorMessages.Add(error.ErrorMessage);
			}
			return errorMessages;
		}
	}
}