using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface IBudgetsService
	{
		Task<IEnumerable<Budget>> GetAllBudgetsAsync();
		Task<Budget> GetBudgetByIdAsync(int id);
		Task<Budget> CreateBudgetAsync(Budget budget);
		Task<Budget> UpdateBudgetAsync(Budget budget);
		Task<bool> DeleteBudgetAsync(int id);
	}

	public class BudgetsService : IBudgetsService
	{
		private readonly TACRMDbContext _dbContext;

		public BudgetsService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Budget>> GetAllBudgetsAsync()
		{
			return await _dbContext.Budgets.ToListAsync();
		}

		public async Task<Budget> GetBudgetByIdAsync(int id)
		{
			return await _dbContext.Budgets.FindAsync(id);
		}

		public async Task<Budget> CreateBudgetAsync(Budget budget)
		{
			_dbContext.Budgets.Add(budget);
			await _dbContext.SaveChangesAsync();
			return budget;
		}

		public async Task<Budget> UpdateBudgetAsync(Budget budget)
		{
			var existingBudget = await _dbContext.Budgets.FindAsync(budget.BudgetID);
			if (existingBudget == null) return null;

			_dbContext.Entry(existingBudget).CurrentValues.SetValues(budget);
			await _dbContext.SaveChangesAsync();
			return existingBudget;
		}

		public async Task<bool> DeleteBudgetAsync(int id)
		{
			var budget = await _dbContext.Budgets.FindAsync(id);
			if (budget == null) return false;

			_dbContext.Budgets.Remove(budget);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
