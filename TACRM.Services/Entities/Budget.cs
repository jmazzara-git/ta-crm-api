
namespace TACRM.Services.Entities
{
	public class Budget
	{
		public int BudgetID { get; set; }
		public int ContactID { get; set; }
		public Contact Contact { get; set; } // Navigation Property
		public string BudgetName { get; set; }
		public string BudgetDetails { get; set; }
		public string FilePath { get; set; }
		public string Currency { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
