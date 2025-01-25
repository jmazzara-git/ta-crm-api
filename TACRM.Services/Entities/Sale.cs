
namespace TACRM.Services.Entities
{
	public class Sale
	{
		public int SaleID { get; set; }
		public int UserID { get; set; } // Foreign Key
		public int ContactID { get; set; } // Foreign Key
		public string SaleName { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public User User { get; set; } // Navigation property
		public Contact Contact { get; set; } // Navigation property
		public ICollection<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();
		public ICollection<SaleTraveler> SaleTravelers { get; set; } = new List<SaleTraveler>();
	}
}
