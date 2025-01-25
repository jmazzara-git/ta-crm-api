
namespace TACRM.Services.Entities
{
	public class ContactStatus
	{
		public int StatusID { get; set; }
		public string StatusName { get; set; }
		public ICollection<Contact> Contacts { get; set; } // Navigation Property
	}
}
