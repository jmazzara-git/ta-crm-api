
namespace TACRM.Services.Entities
{
	public class SaleProduct
	{
		public int SaleProductID { get; set; }
		public int SaleID { get; set; }
		public Sale Sale { get; set; } // Navigation Property
		public int ProductID { get; set; }
		public Product Product { get; set; } // Navigation Property
		public int? ProviderID { get; set; }
		public Provider Provider { get; set; } // Navigation Property
		public string BookingID { get; set; }
		public DateTime? BookingDate { get; set; }
		public DateTime? CheckinDate { get; set; }
		public DateTime? CheckoutDate { get; set; }
		public string Currency { get; set; }
		public decimal BasePrice { get; set; }
		public decimal FinalPrice { get; set; }
		public DateTime? PaymentDueDate { get; set; }
		public decimal Commission { get; set; }
		public string Status { get; set; }
		public string CancellationReason { get; set; }
		public DateTime? CancellationDate { get; set; }
		public ICollection<Payment> Payments { get; set; } // Navigation Property
	}
}
