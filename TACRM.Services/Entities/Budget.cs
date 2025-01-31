namespace TACRM.Services.Entities
{
	public class Budget
	{
		public int BudgetID { get; set; }
		public int UserID { get; set; }
		public int ContactID { get; set; }
		public string BudgetName { get; set; }
		public string BudgetDetails { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		public User User { get; set; }
		public Contact Contact { get; set; }
		public ICollection<BudgetProduct> BudgetProducts { get; set; }
	}
}
