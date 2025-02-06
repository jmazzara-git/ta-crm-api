using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("ContactProduct", Schema = "tacrm")]
	public class ContactProduct
	{
		[Key]
		public int ContactProductId { get; set; }
		public int ContactId { get; set; }
		public int ProductId { get; set; }

		// Navigation Properties
		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; } = null!;
	}
}
