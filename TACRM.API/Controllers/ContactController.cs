using FluentValidation;
using FluentValidation.Results;
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
		public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
		{
			var contacts = await _contactsService.GetAsync();
			return Ok(contacts);
		}

		// GET: api/contact/search
		[HttpGet("search")]
		public async Task<IActionResult> SearchContacts(
			[FromQuery] string searchTerm = null,
			[FromQuery] string contactStatus = null,
			[FromQuery] int pageNumber = 1,
			[FromQuery] int pageSize = 10)
		{
			var (results, totalCount) = await _contactsService.SearchAsync(
				searchTerm,
				contactStatus,
				pageNumber,
				pageSize);

			return Ok(new
			{
				Results = results,
				TotalCount = totalCount,
				PageNumber = pageNumber,
				PageSize = pageSize
			});
		}

		// GET: api/contact/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ContactDto>> GetContact(int id)
		{
			var contact = await _contactsService.GetByIdAsync(id);

			if (contact == null)
				return NotFound("Contact not found");

			return Ok(contact);
		}

		// POST: api/contact
		[HttpPost]
		public async Task<ActionResult<ContactDto>> CreateContact(ContactDto dto)
		{
			if (dto == null)
				return BadRequest();

			try
			{
				var createdContact = await _contactsService.CreateAsync(dto);
				return CreatedAtAction(nameof(GetContact), new { id = createdContact.ContactId }, createdContact);
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

		// PUT: api/contact
		[HttpPut]
		public async Task<IActionResult> UpdateContact(ContactDto dto)
		{
			if (dto == null)
				return BadRequest();

			try
			{
				await _contactsService.UpdateAsync(dto);
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

		// DELETE: api/contact/[id]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			try
			{
				await _contactsService.DeleteAsync(id);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Contact not found");
			}
		}

		private static List<string> ProcessValidationErrors(IEnumerable<ValidationFailure> errors)
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