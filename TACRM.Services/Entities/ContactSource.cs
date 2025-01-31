
namespace TACRM.Services.Entities
{
	public class ContactSource
	{
		public int ContactSourceID { get; set; }
		public string ContactSourceName { get; set; }

		// Navigation properties
		public ICollection<Contact> Contacts { get; set; }
	}
}
