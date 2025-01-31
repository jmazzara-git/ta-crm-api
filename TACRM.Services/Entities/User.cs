
namespace TACRM.Services.Entities
{
	public class User
	{

		public int UserID { get; set; }
		public int? ParentUserID { get; set; }
		public string UserType { get; set; } // AGENT, AGENCY, ADMIN
		public string IdpID { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string UserContactInfo { get; set; }
		public string UserPicturePath { get; set; }
		public string AgencyName { get; set; }
		public string AgencyContactInfo { get; set; }
		public string AgencyPicturePath { get; set; }
		public string DefaultBudgetMessage { get; set; }
		public string DefaultWelcomeMessage { get; set; }
		public string DefaultThanksMessage { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		public User Agency { get; set; }
		public ICollection<User> Agents { get; set; }
		public ICollection<Contact> Contacts { get; set; }
		public ICollection<Product> Products { get; set; }
		public ICollection<Provider> Providers { get; set; }
		public ICollection<Subscription> Subscriptions { get; set; }
		public ICollection<Budget> Budgets { get; set; }
		public ICollection<Sale> Sales { get; set; }
		public ICollection<Event> Events { get; set; }
		public ICollection<Notification> Notifications { get; set; }
	}
}
