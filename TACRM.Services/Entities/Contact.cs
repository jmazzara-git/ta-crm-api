namespace TACRM.Services.Entities
{
	public class Contact
	{
		public int ContactID { get; set; }
		public int UserID { get; set; } // Foreign Key
		public int? ContactSourceID { get; set; } // Nullable Foreign Key
		public int? StatusID { get; set; } // Nullable Foreign Key
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateTime? TravelDateStart { get; set; }
		public DateTime? TravelDateEnd { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; } // JSON or CSV format
		public string Comments { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public User User { get; set; } // Navigation property
		public ContactSource ContactSource { get; set; }
		public ContactStatus Status { get; set; }
	}
}
