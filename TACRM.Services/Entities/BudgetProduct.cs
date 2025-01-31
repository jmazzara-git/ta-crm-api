namespace TACRM.Services.Entities
{
	public class BudgetProduct
	{
		public int BudgetProductID { get; set; }
		public int BudgetID { get; set; }
		public int ProductID { get; set; }
		public string ProductDetails { get; set; }
		public DateTime? BudgetDate { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string Currency { get; set; } // USD, ARS
		public decimal? BasePrice { get; set; }
		public decimal FinalPrice { get; set; }
		public decimal? Commission { get; set; }
		public int? ProviderID { get; set; }
		public string BookingID { get; set; }
		public DateTime? BookingDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public string FilePath { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public Budget Budget { get; set; }
		public Product Product { get; set; }
		public Provider Provider { get; set; }
	}
}
