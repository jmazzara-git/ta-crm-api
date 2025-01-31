namespace TACRM.Services.Entities
{
	public class Subscription
	{
		public int SubscriptionID { get; set; }
		public int UserID { get; set; }
		public string PlanName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Status { get; set; } // Active, Canceled, Expired
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public User User { get; set; }
	}
}
