using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("ProductTypeTranslation", Schema = "tacrm")]
	public class ProductTypeTranslation
	{
		public ProductTypeEnum ProductType { get; set; }
		public string DisplayNameEN { get; set; }
		public string DisplayNameES { get; set; }
	}
}
