
namespace TACRM.Services.Entities
{
	public class Provider
	{
		public int ProviderID { get; set; }
		public int? UserID { get; set; }
		public string ProviderName { get; set; }
		public string ContactInfo { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Properties
		public virtual User User { get; set; }
		public ICollection<SaleProduct> SaleProducts { get; set; }
	}
}
