namespace TACRM.Services.Entities
{
	public class ProductTypeTranslation
	{
		public int TranslationID { get; set; }
		public int ProductTypeID { get; set; }
		public string LanguageCode { get; set; } // en, es
		public string DisplayName { get; set; }

		// Navigation properties
		public ProductType ProductType { get; set; }
	}
}
