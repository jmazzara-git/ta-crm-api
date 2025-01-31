namespace TACRM.Services.Entities
{
	public class Provider
	{
		public int ProviderID { get; set; }
		public int UserID { get; set; }
		public string ProviderName { get; set; }
		public string ProviderDetails { get; set; }
		public bool IsShared { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		public User User { get; set; }
	}
}
