
namespace TACRM.Services.Entities
{
	public class Sale
	{
		public int SaleID { get; set; }
		public int TenantID { get; set; }
		public Tenant Tenant { get; set; } // Navigation Property
		public int ContactID { get; set; }
		public Contact Contact { get; set; } // Navigation Property
		public string SaleName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<SaleProduct> SaleProducts { get; set; } // Navigation Property
		public ICollection<SaleTraveler> SaleTravelers { get; set; } // Navigation Property
	}
}
