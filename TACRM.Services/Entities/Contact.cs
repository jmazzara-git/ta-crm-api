namespace TACRM.Services.Entities
{
	public class Contact
	{
		public int ContactID { get; set; }
		public int TenantID { get; set; } // Foreign Key
		public Tenant Tenant { get; set; } // Navigation Property
		public int? ContactSourceID { get; set; }
		public ContactSource ContactSource { get; set; } // Navigation Property
		public int? StatusID { get; set; }
		public ContactStatus ContactStatus { get; set; } // Navigation Property
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateTime? TravelDateStart { get; set; }
		public DateTime? TravelDateEnd { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public string Comments { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
