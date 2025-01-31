namespace TACRM.Services.Entities
{
	public class ProductType
	{
		public int ProductTypeID { get; set; }
		public string ProductTypeKey { get; set; } // Package, Hotel, Ticket, etc.

		// Navigation properties
		public ICollection<ProductTypeTranslation> Translations { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}
