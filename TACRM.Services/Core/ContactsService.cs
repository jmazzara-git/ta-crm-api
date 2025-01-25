using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface IContactsService
	{
		Task<IEnumerable<Contact>> GetAllContactsAsync();
		Task<Contact> GetContactByIdAsync(int id);
		Task<Contact> CreateContactAsync(Contact contact);
		Task<Contact> UpdateContactAsync(Contact contact);
		Task<bool> DeleteContactAsync(int id);
	}

	public class ContactsService : IContactsService
	{
		private readonly TACRMDbContext _dbContext;

		public ContactsService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Contact>> GetAllContactsAsync()
		{
			return await _dbContext.Contacts.ToListAsync();
		}

		public async Task<Contact> GetContactByIdAsync(int id)
		{
			return await _dbContext.Contacts.FindAsync(id);
		}

		public async Task<Contact> CreateContactAsync(Contact contact)
		{
			_dbContext.Contacts.Add(contact);
			await _dbContext.SaveChangesAsync();
			return contact;
		}

		public async Task<Contact> UpdateContactAsync(Contact contact)
		{
			var existingContact = await _dbContext.Contacts.FindAsync(contact.ContactID);
			if (existingContact == null)
			{
				return null;
			}

			_dbContext.Entry(existingContact).CurrentValues.SetValues(contact);
			await _dbContext.SaveChangesAsync();
			return existingContact;
		}

		public async Task<bool> DeleteContactAsync(int id)
		{
			var contact = await _dbContext.Contacts.FindAsync(id);
			if (contact == null)
			{
				return false;
			}

			_dbContext.Contacts.Remove(contact);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
