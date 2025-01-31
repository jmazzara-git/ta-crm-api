namespace TACRM.Services.Entities
{
	public class SaleProduct
	{
		public int SaleProductID { get; set; }
		public int SaleID { get; set; }
		public int ProductID { get; set; }
		public int? ProviderID { get; set; }
		public string BookingID { get; set; }
		public DateTime? BookingDate { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string Currency { get; set; } // USD, ARS
		public decimal BasePrice { get; set; }
		public decimal FinalPrice { get; set; }
		public DateTime PaymentDueDate { get; set; }
		public decimal Commission { get; set; }
		public string Status { get; set; } // Active, Canceled
		public string CancellationReason { get; set; }
		public DateTime? CancellationDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public Sale Sale { get; set; }
		public Product Product { get; set; }
		public Provider Provider { get; set; }
		public ICollection<Payment> Payments { get; set; }
	}
}
