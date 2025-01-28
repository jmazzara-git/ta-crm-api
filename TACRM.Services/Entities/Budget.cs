
namespace TACRM.Services.Entities
{
	public class Budget
	{
		public int BudgetID { get; set; }
		public int ContactID { get; set; } // Foreign key to Contacts table
		public string BudgetName { get; set; }
		public string BudgetDetails { get; set; }
		public int Adults { get; set; } = 0;
		public int Kids { get; set; } = 0;
		public string KidsAges { get; set; } // Comma-separated list of ages
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Properties
		public virtual Contact Contact { get; set; } = null!;
		public virtual ICollection<BudgetProduct> BudgetProducts { get; set; } = new List<BudgetProduct>();
	}
}
