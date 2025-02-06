using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("Provider", Schema = "tacrm")]
	public class Provider
	{
		[Key]
		public int ProviderId { get; set; }
		public int UserId { get; set; }
		public string ProviderName { get; set; }
		public string ProviderDetails { get; set; }
		public bool IsShared { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}
