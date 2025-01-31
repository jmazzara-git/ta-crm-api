namespace TACRM.Services.Entities
{
	public class ContactStatusTranslation
	{
		public int TranslationID { get; set; }
		public int ContactStatusID { get; set; }
		public string LanguageCode { get; set; } // en, es
		public string DisplayName { get; set; }

		// Navigation properties
		public ContactStatus ContactStatus { get; set; }
	}
}
