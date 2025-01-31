
namespace TACRM.Services.Entities
{
	public class Payment
	{
		public int PaymentID { get; set; }
		public int SaleProductID { get; set; }
		public string Currency { get; set; } // USD, ARS
		public decimal PaymentAmount { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentMethod { get; set; }

		// Navigation properties
		public SaleProduct SaleProduct { get; set; }
	}
}
