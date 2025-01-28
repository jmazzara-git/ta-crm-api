using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Entities;
using TACRM.Services.Resources;

namespace TACRM.Services.Core
{
	public interface IContactService
	{
		Task<IEnumerable<Contact>> GetAllContactsAsync();
		Task<(IEnumerable<Contact>, int)> SearchContactsAsync(string search, int? statusId, int page, int pageSize);
		Task<Contact> GetContactByIdAsync(int id);
		Task<Contact> CreateContactAsync(Contact contact);
		Task<Contact> UpdateContactAsync(Contact contact);
		Task<bool> DeleteContactAsync(int id);
		Task<IEnumerable<ContactStatusTranslation>> GetContactStatusesAsync(string languageCode);
	}

	public class ContactsService(
		TACRMDbContext dbContext,
		IStringLocalizer<ValidationMessages> localizer,
		IUserContext userContext) : IContactService
	{
		private readonly TACRMDbContext _dbContext = dbContext;
		private readonly IStringLocalizer<ValidationMessages> _localizer = localizer;
		private readonly IUserContext _userContext = userContext;

		public async Task<IEnumerable<Contact>> GetAllContactsAsync()
		{
			var userId = _userContext.UserId;
			return await _dbContext.Contacts
				.Where(c => c.UserID == userId)
				.Include(c => c.ContactSource)
				.Include(c => c.ContactStatus)
				.ToListAsync();
		}

		public async Task<(IEnumerable<Contact>, int)> SearchContactsAsync(string search, int? statusId, int page, int pageSize)
		{
			var userId = _userContext.UserId;

			var query = _dbContext.Contacts
				.Where(c => c.UserID == userId)
				.Include(c => c.ContactSource)
				.Include(c => c.ContactStatus)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(c => c.FullName.Contains(search));
			}

			if (statusId.HasValue)
			{
				query = query.Where(c => c.ContactStatusID == statusId.Value);
			}

			var totalRecords = await query.CountAsync();

			var contacts = await query
				.OrderBy(c => c.FullName)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return (contacts, totalRecords);
		}

		public async Task<Contact> GetContactByIdAsync(int id)
		{
			var userId = _userContext.UserId;

			var contact = await _dbContext.Contacts
				.Include(c => c.ContactSource)
				.Include(c => c.ContactStatus)
				.FirstOrDefaultAsync(c => c.ContactID == id && c.UserID == userId);

			return contact;
		}

		public async Task<Contact> CreateContactAsync(Contact contact)
		{
			contact.UserID = _userContext.UserId;

			ValidateContact(contact);

			_dbContext.Contacts.Add(contact);
			await _dbContext.SaveChangesAsync();
			return contact;
		}

		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			var userId = _userContext.UserId;

			var existingContact = await _dbContext.Contacts
				.FirstOrDefaultAsync(c => c.ContactID == contact.ContactID && c.UserID == userId);

			if (existingContact == null)
				return null;

			ValidateContact(contact);

			_dbContext.Entry(existingContact).CurrentValues.SetValues(contact);
			await _dbContext.SaveChangesAsync();
			return existingContact;
		}

		public async Task<bool> DeleteContactAsync(int id)
		{
			var userId = _userContext.UserId;

			var contact = await _dbContext.Contacts
				.FirstOrDefaultAsync(c => c.ContactID == id && c.UserID == userId);

			if (contact == null)
				return false;

			_dbContext.Contacts.Remove(contact);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<ContactStatusTranslation>> GetContactStatusesAsync(string languageCode)
		{
			return await _dbContext.ContactStatusTranslations
				.Where(cst => cst.LanguageCode == languageCode)
				.ToListAsync();
		}

		private void ValidateContact(Contact contact)
		{
			if (contact.EnableWhatsAppNotifications && string.IsNullOrWhiteSpace(contact.Phone))
				throw new InvalidOperationException(_localizer["WhatsAppValidation"]);

			if (contact.EnableEmailNotifications && string.IsNullOrWhiteSpace(contact.Email))
				throw new InvalidOperationException(_localizer["EmailValidation"]);

			if (contact.TravelDateStart > contact.TravelDateEnd)
				throw new InvalidOperationException(_localizer["InvalidDates"]);

			if (contact.Kids > 0 && string.IsNullOrWhiteSpace(contact.KidsAges))
				throw new InvalidOperationException(_localizer["KidsAgesRequired"]);

			if (!string.IsNullOrWhiteSpace(contact.KidsAges) && contact.KidsAges.Split(',').Length != contact.Kids)
				throw new InvalidOperationException(_localizer["KidsAgesMismatch"]);
		}
	}
}
