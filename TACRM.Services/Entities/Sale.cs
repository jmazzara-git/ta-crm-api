using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("Sale", Schema = "tacrm")]
	public class Sale
	{
		[Key]
		public int SaleId { get; set; }
		public int UserId { get; set; }
		public int ContactId { get; set; }
		public Guid SaleGUID { get; set; }
		public string SaleName { get; set; }
		public string SaleDetails { get; set; }
		public DateOnly? StartDate { get; set; }
		public DateOnly? EndDate { get; set; }
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
		public ICollection<SaleTraveler> Travelers { get; set; }
		public ICollection<SaleProduct> Products { get; set; }
	}
}