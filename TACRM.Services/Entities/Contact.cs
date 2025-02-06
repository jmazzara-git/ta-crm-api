using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("Contact", Schema = "tacrm")]
	public class Contact
	{
		[Key]
		public int ContactId { get; set; }
		public int UserId { get; set; }
		public ContactStatusEnum ContactStatus { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateOnly? FromDate { get; set; }
		public DateOnly? ToDate { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public string Comments { get; set; }
		public bool EnableWhatsAppNotifications { get; set; }
		public bool EnableEmailNotifications { get; set; }
		public int? ContactSourceId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		[ForeignKey("UserId")]
		public User User { get; set; }
		[ForeignKey("ContactSourceId")]
		public ContactSource Source { get; set; }
		public ICollection<ContactProduct> Products { get; set; } = [];
		public ICollection<Budget> Budgets { get; set; } = [];
		public ICollection<Sale> Sales { get; set; } = [];
	}
}
