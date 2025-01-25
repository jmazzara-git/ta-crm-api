using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface ISalesService
	{
		Task<IEnumerable<Sale>> GetAllSalesAsync();
		Task<Sale> GetSaleByIdAsync(int id);
		Task<Sale> CreateSaleAsync(Sale sale);
		Task<Sale> UpdateSaleAsync(Sale sale);
		Task<bool> DeleteSaleAsync(int id);
	}

	public class SalesService : ISalesService
	{
		private readonly TACRMDbContext _dbContext;

		public SalesService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Sale>> GetAllSalesAsync()
		{
			return await _dbContext.Sales.ToListAsync();
		}

		public async Task<Sale> GetSaleByIdAsync(int id)
		{
			return await _dbContext.Sales.FindAsync(id);
		}

		public async Task<Sale> CreateSaleAsync(Sale sale)
		{
			_dbContext.Sales.Add(sale);
			await _dbContext.SaveChangesAsync();
			return sale;
		}

		public async Task<Sale> UpdateSaleAsync(Sale sale)
		{
			var existingSale = await _dbContext.Sales.FindAsync(sale.SaleID);
			if (existingSale == null) return null;

			_dbContext.Entry(existingSale).CurrentValues.SetValues(sale);
			await _dbContext.SaveChangesAsync();
			return existingSale;
		}

		public async Task<bool> DeleteSaleAsync(int id)
		{
			var sale = await _dbContext.Sales.FindAsync(id);
			if (sale == null) return false;

			_dbContext.Sales.Remove(sale);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
