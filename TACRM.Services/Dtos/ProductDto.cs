namespace TACRM.Services.Dtos
{
	public class ProductDto
	{
		public int ProductId { get; set; }
		public int UserId { get; set; }
		public string ProductTypeCode { get; set; }
		public string ProductName { get; set; }
		public string ProductDetails { get; set; }
		public bool IsShared { get; set; } = false;
	}
}
