using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using TACRM.Services.Abstractions;
using TACRM.Services.Entities;

namespace TACRM.Services.Business
{
	public class ContactsService(
		TACRMDbContext context,
		UserContext userContext,
		IValidator<Contact> validator) : IContactsService
	{
		private readonly TACRMDbContext _context = context;
		private readonly UserContext _userContext = userContext;
		private readonly IValidator<Contact> _validator = validator;

		public async Task<IEnumerable<Contact>> GetContactsAsync()
		{
			return await _context.Contacts
				.Where(c => c.UserID == _userContext.UserId)
				.ToListAsync();
		}

		public async Task<Contact> GetContactByIdAsync(int id)
		{
			return await _context.Contacts
				.FirstOrDefaultAsync(c => c.ContactID == id && c.UserID == _userContext.UserId);
		}

		public async Task<Contact> CreateContactAsync(Contact contact)
		{
			// Set tenant ID
			contact.UserID = _userContext.UserId;

			// Validate
			ValidationResult result = await _validator.ValidateAsync(contact);
			if (!result.IsValid)
				throw new ValidationException(result.Errors);

			_context.Contacts.Add(contact);
			await _context.SaveChangesAsync();
			return contact;
		}

		public async Task UpdateContactAsync(Contact contact)
		{
			Contact existingContact = await GetContactByIdAsync(contact.ContactID);
			if (existingContact == null)
				throw new NotFoundException("Contact not found.");

			// Validate
			ValidationResult result = await _validator.ValidateAsync(contact);
			if (!result.IsValid)
				throw new ValidationException(result.Errors);

			_context.Entry(existingContact).CurrentValues.SetValues(contact);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteContactAsync(int id)
		{
			Contact contact = await GetContactByIdAsync(id);
			if (contact == null)
				throw new NotFoundException("Contact not found.");

			_context.Contacts.Remove(contact);
			await _context.SaveChangesAsync();
		}

		public async Task<(IEnumerable<Contact> Contacts, int TotalCount)> SearchContactsAsync(
			string searchTerm = null,
			int? contactStatusId = null,
			int pageNumber = 1,
			int pageSize = 10)
		{
			// Base query with tenant filtering
			var query = _context.Contacts
				.Where(c => c.UserID == _userContext.UserId);

			// Apply search term filter (by full name)
			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(c => c.FullName.Contains(searchTerm));
			}

			// Apply contact status filter
			if (contactStatusId.HasValue)
			{
				query = query.Where(c => c.ContactStatusID == contactStatusId.Value);
			}

			// Get the total count of matching records (for pagination)
			int totalCount = await query.CountAsync();

			// Apply pagination
			var contacts = await query
				.OrderBy(c => c.FullName) // Order by full name (or any other field)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return (contacts, totalCount);
		}
	}
}