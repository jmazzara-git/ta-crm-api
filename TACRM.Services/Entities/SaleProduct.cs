using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("SaleProduct", Schema = "tacrm")]
	public class SaleProduct
	{
		[Key]
		public int SaleProductId { get; set; }
		public int SaleId { get; set; }
		public int ProductId { get; set; }
		public string ProductDetails { get; set; }
		public int ProviderID { get; set; }
		public DateOnly SaleDate { get; set; }
		public DateOnly? FromDate { get; set; }
		public DateOnly? ToDate { get; set; }
		public string Currency { get; set; }
		public decimal BasePrice { get; set; }
		public decimal FinalPrice { get; set; }
		public decimal Commission { get; set; }
		public string BookingID { get; set; }
		public DateOnly? BookingDate { get; set; }
		public DateOnly PaymentDueDate { get; set; }
		public bool IsCancelled { get; set; }
		public string CancellationReason { get; set; }
		public DateOnly? CancellationDate { get; set; }
		public bool IsFullPaid { get; set; }
		public DateOnly? FullPaidDate { get; set; }
		public bool IsComissionPaid { get; set; }
		public DateOnly? ComissionPaidDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		[ForeignKey("SaleId")]
		public Sale Sale { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
		[ForeignKey("ProviderID")]
		public Provider Provider { get; set; }
		public ICollection<SaleProductPayment> Payments { get; set; }
	}
}