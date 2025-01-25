
namespace TACRM.Services.Entities
{
	public class ContactProductInterest
	{
		public int ContactProductInterestID { get; set; }
		public int ContactID { get; set; } // Foreign key to Contacts
		public int ProductID { get; set; } // Foreign key to Products

		// Navigation Properties
		public virtual Contact Contact { get; set; } = null!;
		public virtual Product Product { get; set; } = null!;
	}
}
