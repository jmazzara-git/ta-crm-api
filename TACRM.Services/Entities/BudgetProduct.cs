namespace TACRM.Services.Entities
{
	public class BudgetProduct
	{
		public int BudgetProductID { get; set; }
		public int BudgetID { get; set; } // Foreign key to Budgets table
		public int ProductID { get; set; } // Foreign key to Products table
		public int? ProviderID { get; set; } // Foreign key to Providers table (optional)
		public string ProductDetails { get; set; }
		public decimal? BasePrice { get; set; } // Nullable base price
		public decimal FinalPrice { get; set; } // Final price (required)
		public string Currency { get; set; } = "USD"; // Currency for the product price
		public string FilePath { get; set; }
		public decimal? Commission { get; set; }
		public string BookingID { get; set; } // Temporary booking identifier
		public DateTime? BookingDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public DateTime? BudgetDate { get; set; }
		public DateTime? CheckinDate { get; set; }
		public DateTime? CheckoutDate { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Properties
		public virtual Budget Budget { get; set; }
		public virtual Product Product { get; set; }
		public virtual Provider Provider { get; set; }
	}
}
