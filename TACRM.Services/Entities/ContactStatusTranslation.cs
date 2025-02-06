using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("ContactStatusTranslation", Schema = "tacrm")]
	public class ContactStatusTranslation
	{
		public ContactStatusEnum ContactStatus { get; set; }
		public string DisplayNameEN { get; set; }
		public string DisplayNameES { get; set; }
	}
}
