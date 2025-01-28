namespace TACRM.Services.Entities
{
	public class ProductType
	{
		public int ProductTypeID { get; set; }
		public string ProductTypeKey { get; set; } // Key (e.g., "Package", "Hotel")

		// Navigation Properties
		public virtual ICollection<ProductTypeTranslation> Translations { get; set; } = [];
	}

	public class ProductTypeTranslation
	{
		public int TranslationID { get; set; }
		public int ProductTypeID { get; set; } // Foreign key to ProductType
		public string LanguageCode { get; set; } // e.g., 'en', 'es'
		public string DisplayName { get; set; } // Localized name of the product type

		// Navigation Properties
		public virtual ProductType ProductType { get; set; }
	}
}
