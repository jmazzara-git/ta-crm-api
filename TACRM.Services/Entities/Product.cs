
namespace TACRM.Services.Entities
{
	public class Product
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public ICollection<ContactProductInterest> ContactProductInterests { get; set; } // Navigation Property
		public ICollection<SaleProduct> SaleProducts { get; set; } // Navigation Property
	}
}
