using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface ISaleProductsService
	{
		Task<IEnumerable<SaleProduct>> GetAllSaleProductsAsync();
		Task<SaleProduct> GetSaleProductByIdAsync(int id);
		Task<SaleProduct> CreateSaleProductAsync(SaleProduct saleProduct);
		Task<SaleProduct> UpdateSaleProductAsync(SaleProduct saleProduct);
		Task<bool> DeleteSaleProductAsync(int id);
	}

	public class SaleProductsService : ISaleProductsService
	{
		private readonly TACRMDbContext _dbContext;

		public SaleProductsService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<SaleProduct>> GetAllSaleProductsAsync()
		{
			return await _dbContext.SaleProducts.ToListAsync();
		}

		public async Task<SaleProduct> GetSaleProductByIdAsync(int id)
		{
			return await _dbContext.SaleProducts.FindAsync(id);
		}

		public async Task<SaleProduct> CreateSaleProductAsync(SaleProduct saleProduct)
		{
			_dbContext.SaleProducts.Add(saleProduct);
			await _dbContext.SaveChangesAsync();
			return saleProduct;
		}

		public async Task<SaleProduct> UpdateSaleProductAsync(SaleProduct saleProduct)
		{
			var existingSaleProduct = await _dbContext.SaleProducts.FindAsync(saleProduct.SaleProductID);
			if (existingSaleProduct == null) return null;

			_dbContext.Entry(existingSaleProduct).CurrentValues.SetValues(saleProduct);
			await _dbContext.SaveChangesAsync();
			return existingSaleProduct;
		}

		public async Task<bool> DeleteSaleProductAsync(int id)
		{
			var saleProduct = await _dbContext.SaleProducts.FindAsync(id);
			if (saleProduct == null) return false;

			_dbContext.SaleProducts.Remove(saleProduct);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
