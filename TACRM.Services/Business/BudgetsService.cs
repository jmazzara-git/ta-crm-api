using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Entities;
using TACRM.Services.Resources;

namespace TACRM.Services.Core
{
	public interface IBudgetsService
	{
		Task<Budget> CreateBudgetAsync(Budget budget);
		Task<Budget> UpdateBudgetAsync(Budget budget);
		Task<bool> DeleteBudgetAsync(int budgetId);
		Task<Budget> GetBudgetByIdAsync(int budgetId);
		Task<IEnumerable<Budget>> GetBudgetsByContactIdAsync(int contactId);
		Task<(IEnumerable<Budget>, int)> SearchBudgetsAsync(string budgetName, int? contactId, int page, int pageSize);
	}

	public class BudgetsService : IBudgetsService
	{
		private readonly TACRMDbContext _dbContext;
		private readonly IStringLocalizer<ValidationMessages> _localizer;
		private readonly IUserContext _userContext;

		public BudgetsService(TACRMDbContext dbContext, IStringLocalizer<ValidationMessages> localizer, IUserContext userContext)
		{
			_dbContext = dbContext;
			_localizer = localizer;
			_userContext = userContext;
		}

		// Search budgets with filters and pagination
		public async Task<(IEnumerable<Budget>, int)> SearchBudgetsAsync(string budgetName, int? contactId, int page, int pageSize)
		{
			var userId = _userContext.UserId;

			var query = _dbContext.Budgets
				.Include(b => b.Contact)
				.Include(b => b.BudgetProducts)
				.Where(b => b.Contact.UserID == userId)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(budgetName))
			{
				query = query.Where(b => b.BudgetName.Contains(budgetName));
			}

			if (contactId.HasValue)
			{
				query = query.Where(b => b.ContactID == contactId.Value);
			}

			var totalRecords = await query.CountAsync();

			var budgets = await query
				.OrderBy(b => b.BudgetName)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return (budgets, totalRecords);
		}

		public async Task<Budget> CreateBudgetAsync(Budget budget)
		{
			ValidateBudget(budget);

			budget.CreatedAt = DateTime.UtcNow;
			budget.UpdatedAt = DateTime.UtcNow;

			_dbContext.Budgets.Add(budget);
			await _dbContext.SaveChangesAsync();
			return budget;
		}

		public async Task<Budget> UpdateBudgetAsync(Budget budget)
		{
			var existingBudget = await _dbContext.Budgets
				.Include(b => b.BudgetProducts)
				.FirstOrDefaultAsync(b => b.BudgetID == budget.BudgetID && b.Contact.UserID == _userContext.UserId);

			if (existingBudget == null) return null;

			ValidateBudget(budget);

			_dbContext.Entry(existingBudget).CurrentValues.SetValues(budget);
			await _dbContext.SaveChangesAsync();
			return existingBudget;
		}

		public async Task<bool> DeleteBudgetAsync(int budgetId)
		{
			var budget = await _dbContext.Budgets
				.Include(b => b.Contact)
				.FirstOrDefaultAsync(b => b.BudgetID == budgetId && b.Contact.UserID == _userContext.UserId);

			if (budget == null) return false;

			_dbContext.Budgets.Remove(budget);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<Budget> GetBudgetByIdAsync(int budgetId)
		{
			return await _dbContext.Budgets
				.Include(b => b.BudgetProducts)
				.Include(b => b.Contact)
				.FirstOrDefaultAsync(b => b.BudgetID == budgetId && b.Contact.UserID == _userContext.UserId);
		}

		public async Task<IEnumerable<Budget>> GetBudgetsByContactIdAsync(int contactId)
		{
			return await _dbContext.Budgets
				.Include(b => b.BudgetProducts)
				.Where(b => b.ContactID == contactId && b.Contact.UserID == _userContext.UserId)
				.ToListAsync();
		}

		private void ValidateBudget(Budget budget)
		{
			if (budget.Kids > 0 && string.IsNullOrWhiteSpace(budget.KidsAges))
			{
				throw new InvalidOperationException(_localizer["KidsAgesRequired"]);
			}

			if (!string.IsNullOrWhiteSpace(budget.KidsAges) && budget.KidsAges.Split(',').Length != budget.Kids)
			{
				throw new InvalidOperationException(_localizer["KidsAgesMismatch"]);
			}
		}
	}
}
