using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("Budget", Schema = "tacrm")]
	public class Budget
	{
		[Key]
		public int BudgetId { get; set; }
		public int UserId { get; set; }
		public int ContactId { get; set; }
		public string BudgetGUID { get; set; }
		public string BudgetName { get; set; }
		public string BudgetDetails { get; set; }
		public int Adults { get; set; }
		public int Kids { get; set; }
		public string KidsAges { get; set; }
		public bool IsSent { get; set; }
		public DateOnly? SentDate { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		// Navigation properties
		[ForeignKey("UserId")]
		public User User { get; set; }
		[ForeignKey("ContactId")]
		public Contact Contact { get; set; }
		public ICollection<BudgetProduct> Products { get; set; }
	}
}