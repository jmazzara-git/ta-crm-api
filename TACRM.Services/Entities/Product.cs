namespace TACRM.Services.Entities
{
	public class Product
	{
		public int ProductID { get; set; }
		public int UserID { get; set; }
		public int ProductTypeID { get; set; }
		public string ProductName { get; set; }
		public string ProductDetails { get; set; }
		public bool IsShared { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDisabled { get; set; }

		// Navigation properties
		public User User { get; set; }
		public ProductType ProductType { get; set; }
	}
}
