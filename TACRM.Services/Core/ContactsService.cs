using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Entities;
using TACRM.Services.Resources;

namespace TACRM.Services.Core
{
	public interface IContactsService
	{
		Task<IEnumerable<Contact>> GetAllContactsAsync();
		Task<Contact> GetContactByIdAsync(int id);
		Task<Contact> CreateContactAsync(Contact contact);
		Task<Contact> UpdateContactAsync(Contact contact);
		Task<bool> DeleteContactAsync(int id);
		Task<IEnumerable<ContactStatusTranslation>> GetContactStatusesAsync(string languageCode);
		Task AddProductInterestAsync(int contactId, int productId);
		Task RemoveProductInterestAsync(int contactProductInterestId);
	}

	public class ContactsService(TACRMDbContext dbContext, IStringLocalizer<ValidationMessages> localizer) : IContactsService
	{
		private readonly TACRMDbContext _dbContext = dbContext;
		private readonly IStringLocalizer<ValidationMessages> _localizer = localizer;

		public async Task<IEnumerable<Contact>> GetAllContactsAsync()
		{
			return await _dbContext.Contacts
				.Include(c => c.ContactSource)
				.Include(c => c.ContactStatus)
				.Include(c => c.ProductInterests)
				.ThenInclude(pi => pi.Product)
				.ToListAsync();
		}

		public async Task<Contact> GetContactByIdAsync(int id)
		{
			return await _dbContext.Contacts
				.Include(c => c.ContactSource)
				.Include(c => c.ContactStatus)
				.Include(c => c.ProductInterests)
				.ThenInclude(pi => pi.Product)
				.FirstOrDefaultAsync(c => c.ContactID == id);
		}

		public async Task<Contact> CreateContactAsync(Contact contact)
		{
			// Validation rules for WhatsApp and Email notifications
			if (contact.EnableWhatsAppNotifications && string.IsNullOrWhiteSpace(contact.Phone))
				throw new InvalidOperationException(_localizer["WhatsAppValidation"]);

			if (contact.EnableEmailNotifications && string.IsNullOrWhiteSpace(contact.Email))
				throw new InvalidOperationException(_localizer["EmailValidation"]);

			if (contact.TravelDateStart > contact.TravelDateEnd)
				throw new InvalidOperationException(_localizer["InvalidDates"]);

			// Check for duplicate email for the same user
			if (await _dbContext.Contacts.AnyAsync(c => c.UserID == contact.UserID && c.Email == contact.Email))
				throw new InvalidOperationException(_localizer["DuplicateContact"]);

			_dbContext.Contacts.Add(contact);
			await _dbContext.SaveChangesAsync();
			return contact;
		}

		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			var existingContact = await _dbContext.Contacts.FindAsync(contact.ContactID);
			if (existingContact == null)
				return null;

			// Validation rules
			if (contact.EnableWhatsAppNotifications && string.IsNullOrWhiteSpace(contact.Phone))
				throw new InvalidOperationException(_localizer["WhatsAppValidation"]);

			if (contact.EnableEmailNotifications && string.IsNullOrWhiteSpace(contact.Email))
				throw new InvalidOperationException(_localizer["EmailValidation"]);

			if (contact.TravelDateStart > contact.TravelDateEnd)
				throw new InvalidOperationException(_localizer["InvalidDates"]);

			_dbContext.Entry(existingContact).CurrentValues.SetValues(contact);
			await _dbContext.SaveChangesAsync();
			return existingContact;
		}

		public async Task<bool> DeleteContactAsync(int id)
		{
			var contact = await _dbContext.Contacts.FindAsync(id);
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

		public async Task AddProductInterestAsync(int contactId, int productId)
		{
			// Ensure the contact exists
			var contactExists = await _dbContext.Contacts.AnyAsync(c => c.ContactID == contactId);
			if (!contactExists)
				throw new InvalidOperationException(_localizer["ContactNotFound"]);

			// Ensure the product exists
			var productExists = await _dbContext.Products.AnyAsync(p => p.ProductID == productId);
			if (!productExists)
				throw new InvalidOperationException(_localizer["ProductNotFound"]);

			var newInterest = new ContactProductInterest
			{
				ContactID = contactId,
				ProductID = productId
			};

			_dbContext.ContactProductInterests.Add(newInterest);
			await _dbContext.SaveChangesAsync();
		}

		public async Task RemoveProductInterestAsync(int contactProductInterestId)
		{
			var interest = await _dbContext.ContactProductInterests.FindAsync(contactProductInterestId);
			if (interest == null)
				throw new InvalidOperationException(_localizer["ProductInterestNotFound"]);

			_dbContext.ContactProductInterests.Remove(interest);
			await _dbContext.SaveChangesAsync();
		}
	}
}
