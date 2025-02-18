namespace TACRM.Services.Dtos
{
	public class BudgetDto
	{
		public int BudgetId { get; set; }
		public int UserId { get; set; }
		public int ContactId { get; set; }
		public string BudgetGUID { get; set; }
		public string BudgetName { get; set; }
		public string BudgetDetails { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public int[] KidsAges { get; set; } = [];
		public bool IsSent { get; set; }
		public DateOnly? SentDate { get; set; }
	}
}
