
namespace TACRM.Services.Entities
{
	public class Event
	{
		public int EventID { get; set; }
		public int UserID { get; set; }
		public string EventType { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }
		public bool IsCustom { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public User User { get; set; }
		public ICollection<Notification> Notifications { get; set; }
	}
}
