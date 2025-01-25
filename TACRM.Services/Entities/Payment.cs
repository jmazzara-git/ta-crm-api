
namespace TACRM.Services.Entities
{
	public class Payment
	{
		public int PaymentID { get; set; }
		public int SaleProductID { get; set; }
		public SaleProduct SaleProduct { get; set; } // Navigation Property
		public string Currency { get; set; }
		public decimal PaymentAmount { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentMethod { get; set; }
	}
}
