namespace TACRM.Services.Entities
{
	public class Contact
	{
		public int ContactID { get; set; }
		public int UserID { get; set; } // Reference to the Agent/Owner
		public int? ContactSourceID { get; set; } // Source of the contact (e.g., WhatsApp)
		public int ContactStatusID { get; set; } // Current status of the contact
		public string FullName { get; set; } // Full name of the contact
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateTime? TravelDateStart { get; set; } // Planned travel start date
		public DateTime? TravelDateEnd { get; set; } // Planned travel end date
		public int Adults { get; set; } = 0; // Number of adults
		public int Kids { get; set; } = 0; // Number of kids
		public string KidsAges { get; set; } // CSV for ages
		public string Comments { get; set; } // Free-text additional information
		public bool EnableWhatsAppNotifications { get; set; } = false; // Flag for WhatsApp notifications
		public bool EnableEmailNotifications { get; set; } = false; // Flag for email notifications
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Properties
		public virtual User User { get; set; }
		public virtual ContactSource ContactSource { get; set; }
		public virtual ContactStatus ContactStatus { get; set; }
		public virtual ICollection<ContactProductInterest> ProductInterests { get; set; } = [];
	}
}
