
namespace TACRM.Services.Entities
{
	public class Provider
	{
		public int ProviderID { get; set; }
		public string Name { get; set; }
		public string ContactInfo { get; set; }
		public ICollection<SaleProduct> SaleProducts { get; set; } // Navigation Property
	}
}
