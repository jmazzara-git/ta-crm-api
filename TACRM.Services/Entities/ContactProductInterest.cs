
namespace TACRM.Services.Entities
{
	public class ContactProductInterest
	{
		public int ContactProductInterestID { get; set; }
		public int ContactID { get; set; }
		public Contact Contact { get; set; } // Navigation Property
		public int ProductID { get; set; }
		public Product Product { get; set; } // Navigation Property
	}
}
