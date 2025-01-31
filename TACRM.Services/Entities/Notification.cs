namespace TACRM.Services.Entities
{
	public class Notification
	{
		public int NotificationID { get; set; }
		public int UserID { get; set; }
		public string NotificationType { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; }
		public int? EventID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public User User { get; set; }
		public Event Event { get; set; }
	}
}
