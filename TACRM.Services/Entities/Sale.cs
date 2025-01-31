namespace TACRM.Services.Entities
{
	public class Sale
	{
		public int SaleID { get; set; }
		public int UserID { get; set; }
		public int ContactID { get; set; }
		public Guid SaleGUID { get; set; }
		public string SaleName { get; set; }
		public string SaleDetails { get; set; }
		public DateTime? StartDate { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public User User { get; set; }
		public Contact Contact { get; set; }
		public ICollection<SaleTraveler> SaleTravelers { get; set; }
		public ICollection<SaleProduct> SaleProducts { get; set; }
	}
}
