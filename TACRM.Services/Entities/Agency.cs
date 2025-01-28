namespace TACRM.Services.Entities
{
	public class Agency
	{
		public int AgencyID { get; set; }
		public string AgencyName { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<User> Users { get; set; } = [];
	}
}
