namespace TACRM.Services.Entities
{
	public class Contact
	{
		public int ContactID { get; set; }
		public int UserID { get; set; }
		public int? ContactSourceID { get; set; }
		public int ContactStatusID { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public string Comments { get; set; }
		public bool EnableWhatsAppNotifications { get; set; }
		public bool EnableEmailNotifications { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		public User User { get; set; }
		public ContactSource Source { get; set; }
		public ContactStatus Status { get; set; }
		public ICollection<ContactProduct> Products { get; set; }
		public ICollection<Budget> Budgets { get; set; }
		public ICollection<Sale> Sales { get; set; }
	}
}
