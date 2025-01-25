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
		private readonly IUserContext _userContext;

		public SalesService(TACRMDbContext dbContext, IUserContext userContext)
		{
			_dbContext = dbContext;
			_userContext = userContext;
		}

		public async Task<IEnumerable<Sale>> GetAllSalesAsync()
		{
			var userId = _userContext.GetUserId();
			var userType = _userContext.GetUserType();
			var agencyId = _userContext.GetAgencyId();

			if (userType == "Admin")
			{
				return await _dbContext.Sales
					.Include(s => s.User)
					.Include(s => s.Contact)
					.ToListAsync();
			}

			if (userType == "Agency Owner" && agencyId.HasValue)
			{
				return await _dbContext.Sales
					.Include(s => s.User)
					.Include(s => s.Contact)
					.Where(s => s.User.AgencyID == agencyId)
					.ToListAsync();
			}

			return await _dbContext.Sales
				.Include(s => s.User)
				.Include(s => s.Contact)
				.Where(s => s.UserID == userId)
				.ToListAsync();
		}

		public async Task<Sale> GetSaleByIdAsync(int id)
		{
			var userId = _userContext.GetUserId();
			var userType = _userContext.GetUserType();
			var agencyId = _userContext.GetAgencyId();

			var query = _dbContext.Sales
				.Include(s => s.User)
				.Include(s => s.Contact)
				.Where(s => s.SaleID == id);

			if (userType == "Admin")
			{
				return await query.FirstOrDefaultAsync();
			}

			if (userType == "Agency Owner" && agencyId.HasValue)
			{
				return await query.Where(s => s.User.AgencyID == agencyId).FirstOrDefaultAsync();
			}

			return await query.Where(s => s.UserID == userId).FirstOrDefaultAsync();
		}

		public async Task<Sale> CreateSaleAsync(Sale sale)
		{
			sale.UserID = _userContext.GetUserId();

			_dbContext.Sales.Add(sale);
			await _dbContext.SaveChangesAsync();
			return sale;
		}

		public async Task<Sale> UpdateSaleAsync(Sale sale)
		{
			var userId = _userContext.GetUserId();

			var existingSale = await _dbContext.Sales.FindAsync(sale.SaleID);
			if (existingSale == null) throw new KeyNotFoundException("Sale not found");

			if (existingSale.UserID != userId) throw new UnauthorizedAccessException("You cannot update this sale");

			_dbContext.Entry(existingSale).CurrentValues.SetValues(sale);
			await _dbContext.SaveChangesAsync();
			return existingSale;
		}

		public async Task<bool> DeleteSaleAsync(int id)
		{
			var userId = _userContext.GetUserId();
			var userType = _userContext.GetUserType();
			var agencyId = _userContext.GetAgencyId();

			var sale = await GetSaleByIdAsync(id);

			if (sale == null) return false;

			if (userType == "Agent" && sale.UserID != userId) throw new UnauthorizedAccessException("You cannot delete this sale");

			_dbContext.Sales.Remove(sale);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
