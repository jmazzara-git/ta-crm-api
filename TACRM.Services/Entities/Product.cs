using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("Product", Schema = "tacrm")]
	public class Product
	{
		[Key]
		public int ProductId { get; set; }
		public int UserId { get; set; }
		public string ProductTypeCode { get; set; }
		public string ProductName { get; set; }
		public string ProductDetails { get; set; }
		public bool IsShared { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}
