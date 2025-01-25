namespace TACRM.Services.Entities
{
	public class Subscription
	{
		public int SubscriptionID { get; set; }
		public int UserID { get; set; } // Foreign Key
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; } // Nullable for active subscriptions
		public string Status { get; set; } // Active, Cancelled, Expired
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public User User { get; set; } // Navigation property
	}
}
