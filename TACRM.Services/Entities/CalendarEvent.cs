
namespace TACRM.Services.Entities
{
	public class CalendarEvent
	{
		public int EventID { get; set; }
		public int UserID { get; set; } // Foreign Key
		public string EventType { get; set; } // e.g., Trip, Payment, Reminder
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; } // Nullable for single-point events
		public bool IsCustom { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public User User { get; set; } // Navigation property
	}
}
