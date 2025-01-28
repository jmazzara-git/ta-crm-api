
namespace TACRM.Services.Entities
{
	public class Product
	{
		public int ProductID { get; set; }
		public int? UserID { get; set; } // Foreign key to Users (nullable for shared products)
		public int ProductTypeID { get; set; } // Foreign key to ProductType
		public string ProductName { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation Properties
		public virtual ProductType ProductType { get; set; }
		public virtual User User { get; set; }
		public ICollection<SaleProduct> SaleProducts { get; set; }
	}
}
