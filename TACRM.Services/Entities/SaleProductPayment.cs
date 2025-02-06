using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("SaleProductPayment", Schema = "tacrm")]
	public class SaleProductPayment
	{
		[Key]
		public int SaleProductPaymentId { get; set; }
		public int SaleProductId { get; set; }
		public string Currency { get; set; }
		public decimal PaymentAmount { get; set; }
		public DateOnly PaymentDate { get; set; }
		public string PaymentMethod { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
