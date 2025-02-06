using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACRM.Services.Entities
{
	[Table("SaleTraveler", Schema = "tacrm")]
	public class SaleTraveler
	{
		[Key]
		public int SaleTravelerId { get; set; }
		public int SaleId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int? Age { get; set; }
		public string SpecialRequirements { get; set; }
		public bool IsPrimary { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
