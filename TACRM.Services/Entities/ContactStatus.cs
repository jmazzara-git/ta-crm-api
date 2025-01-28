
namespace TACRM.Services.Entities
{

	public class ContactStatus
	{
		public int ContactStatusID { get; set; } // Primary key
		public string CoontactStatusKey { get; set; } // Internal identifier for the status (e.g., "New", "InProgress")

		// Navigation Properties
		public virtual ICollection<ContactStatusTranslation> Translations { get; set; } = [];
	}

	public class ContactStatusTranslation
	{
		public int TranslationID { get; set; }
		public int ContactStatusID { get; set; } // Foreign key to ContactStatus
		public string LanguageCode { get; set; } = string.Empty; // Language code (e.g., 'en', 'es')
		public string DisplayName { get; set; } = string.Empty; // Translated status name

		// Navigation Properties
		public virtual ContactStatus ContactStatus { get; set; } = null!;
	}
}
