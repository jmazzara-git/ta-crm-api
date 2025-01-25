
namespace TACRM.Services.Entities
{
	public class User
	{
		public int UserID { get; set; }
		public int? AgencyID { get; set; } // Nullable to allow users without an agency
		public string Email { get; set; }
		public string FullName { get; set; }
		public string UserType { get; set; } // Agent, Agency Owner, Admin
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public Agency Agency { get; set; } // Navigation property
		public ICollection<Subscription> Subscriptions { get; set; } = [];
		public ICollection<Contact> Contacts { get; set; } = [];
		public ICollection<Sale> Sales { get; set; } = [];
		public ICollection<CalendarEvent> CalendarEvents { get; set; } = [];
		public ICollection<Notification> Notifications { get; set; } = [];
	}
}
