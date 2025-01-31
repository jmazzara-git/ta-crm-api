namespace TACRM.Services.Entities
{
	public class SaleTraveler
	{
		public int SaleTravelerID { get; set; }
		public int SaleID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int? Age { get; set; }
		public string SpecialRequirements { get; set; }
		public bool IsPrimary { get; set; }

		// Navigation properties
		public Sale Sale { get; set; }
	}
}
