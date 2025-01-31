namespace TACRM.Services.Entities
{
	public class ContactStatus
	{
		public int ContactStatusID { get; set; }
		public string ContactStatusKey { get; set; } // New, InProgress, Converted, Future, Lost

		// Navigation properties
		public ICollection<ContactStatusTranslation> Translations { get; set; }
		public ICollection<Contact> Contacts { get; set; }
	}
}
