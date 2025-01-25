
namespace TACRM.Services.Entities
{
	public class User
	{
		public int UserID { get; set; }
		public int TenantID { get; set; }
		public Tenant Tenant { get; set; } // Navigation Property
		public string Email { get; set; }
		public string FullName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
