using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("User", Schema = "tacrm")]
	public class User
	{
		[Key]
		public int UserId { get; set; }
		public int? AgencyUserId { get; set; }
		public UserTypeEnum UserType { get; set; }
		public string IdpId { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string UserContactInfo { get; set; }
		public string UserPicturePath { get; set; }
		public string AgencyName { get; set; }
		public string AgencyContactInfo { get; set; }
		public string AgencyPicturePath { get; set; }
		public string AboutMessage { get; set; }
		public string WelcomeMessage { get; set; }
		public string BudgetMessage { get; set; }
		public string ThanksMessage { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		[ForeignKey("AgencyUserId")]
		public User Agency { get; set; }
		public ICollection<User> Agents { get; set; }
		public ICollection<Contact> Contacts { get; set; }
		public ICollection<Product> Products { get; set; }
		public ICollection<Provider> Providers { get; set; }
		public ICollection<Budget> Budgets { get; set; }
		public ICollection<Sale> Sales { get; set; }
	}
}