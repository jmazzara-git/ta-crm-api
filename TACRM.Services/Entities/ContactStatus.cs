
namespace TACRM.Services.Entities
{

	public class ContactStatus
	{
		public int StatusID { get; set; } // Primary key
		public string Key { get; set; } // Internal identifier for the status (e.g., "New", "InProgress")

		// Navigation Property
		public virtual ICollection<ContactStatusTranslation> Translations { get; set; } = new List<ContactStatusTranslation>();
	}

	public class ContactStatusTranslation
	{
		public int TranslationID { get; set; }
		public int StatusID { get; set; } // Foreign key to ContactStatus
		public string LanguageCode { get; set; } = string.Empty; // Language code (e.g., 'en', 'es')
		public string DisplayName { get; set; } = string.Empty; // Translated status name

		// Navigation Property
		public virtual ContactStatus ContactStatus { get; set; } = null!;
	}
}
