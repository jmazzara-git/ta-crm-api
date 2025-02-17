using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("ContactSource", Schema = "tacrm")]
	public class ContactSource
	{
		[Key]
		public int ContactSourceId { get; set; }
		public string ContactSourceNameEN { get; set; }
		public string ContactSourceNameES { get; set; }
	}
}
