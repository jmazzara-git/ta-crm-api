using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("BudgetProduct", Schema = "tacrm")]
	public class BudgetProduct
	{
		[Key]
		public int BudgetProductId { get; set; }
		public int BudgetId { get; set; }
		public int ProductId { get; set; }
		public string ProductDetails { get; set; }
		public int ProviderID { get; set; }
		public DateOnly BudgetDate { get; set; }
		public DateOnly? FromDate { get; set; }
		public DateOnly? ToDate { get; set; }
		public string Currency { get; set; }
		public decimal? BasePrice { get; set; }
		public decimal FinalPrice { get; set; }
		public decimal? Commission { get; set; }
		public string BookingID { get; set; }
		public DateOnly? BookingDate { get; set; }
		public DateOnly? ExpirationDate { get; set; }
		public string FilePath { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		[ForeignKey("BudgetId")]
		public Budget Budget { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
		[ForeignKey("ProviderID")]
		public Provider Provider { get; set; }
	}
}