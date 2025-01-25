
namespace TACRM.Services.Entities
{
	public class CalendarEvent
	{
		public int EventID { get; set; }
		public int TenantID { get; set; }
		public Tenant Tenant { get; set; } // Navigation Property
		public string EventType { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }
		public bool IsCustom { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? CreatedBy { get; set; }
		public User CreatedByUser { get; set; } // Navigation Property
	}
}
