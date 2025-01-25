
namespace TACRM.Services.Entities
{
	public class ContactSource
	{
		public int ContactSourceID { get; set; }
		public string SourceName { get; set; }
		public ICollection<Contact> Contacts { get; set; } // Navigation Property
	}
}
