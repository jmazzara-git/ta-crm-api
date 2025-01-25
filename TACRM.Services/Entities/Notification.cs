namespace TACRM.Services.Entities
{
	public class Notification
	{
		public int NotificationID { get; set; }
		public int UserID { get; set; } // Foreign key to Users table
		public string Message { get; set; } // Notification message
		public string Type { get; set; } // Type of notification (e.g., Reminder, Alert)
		public int? EntityID { get; set; } // Related entity ID (optional)
		public string EntityType { get; set; } // Type of related entity (optional)
		public bool IsRead { get; set; } = false; // Whether the notification has been read
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Notification timestamp

		public User User { get; set; } // Navigation property to User
	}
}
