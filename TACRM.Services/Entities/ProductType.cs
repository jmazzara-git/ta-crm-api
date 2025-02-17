using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("ProductType", Schema = "tacrm")]
	public class ProductType
	{
		[Key]
		public string ProductTypeCode { get; set; }
		public string ProductTypeNameEN { get; set; }
		public string ProductTypeNameES { get; set; }
	}
}
