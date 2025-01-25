namespace TACRM.Services.Entities
{
	public class Tenant
	{
		public int TenantID { get; set; }
		public string TenantName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<Contact> Contacts { get; set; } // Navigation Property
	}
}
