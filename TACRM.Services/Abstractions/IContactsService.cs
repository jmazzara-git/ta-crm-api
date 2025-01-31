using TACRM.Services.Entities;

namespace TACRM.Services.Abstractions
{
	public interface IContactsService
	{
		Task<IEnumerable<Contact>> GetContactsAsync();
		Task<Contact> GetContactByIdAsync(int id);
		Task<Contact> CreateContactAsync(Contact contact);
		Task UpdateContactAsync(Contact contact);
		Task DeleteContactAsync(int id);
		Task<(IEnumerable<Contact> Contacts, int TotalCount)> SearchContactsAsync(
			string searchTerm = null,
			int? contactStatusId = null,
			int pageNumber = 1,
			int pageSize = 10);
	}
}