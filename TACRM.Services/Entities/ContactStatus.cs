using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("ContactStatus", Schema = "tacrm")]
	public class ContactStatus
	{
		[Key]
		public string ContactStatusCode { get; set; }
		public string ContactStatusNameEN { get; set; }
		public string ContactStatusNameES { get; set; }
	}
}