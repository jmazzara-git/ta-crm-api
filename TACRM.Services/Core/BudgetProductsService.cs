using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TACRM.Services.Entities;
using TACRM.Services.Resources;

namespace TACRM.Services.Core
{
	public interface IBudgetProductsService
	{
		Task<BudgetProduct> AddProductToBudgetAsync(BudgetProduct budgetProduct);
		Task<bool> RemoveProductFromBudgetAsync(int budgetProductId);
	}

	public class BudgetProductsService : IBudgetProductsService
	{
		private readonly TACRMDbContext _dbContext;
		private readonly IStringLocalizer<ValidationMessages> _localizer;
		private readonly IUserContext _userContext;

		public BudgetProductsService(TACRMDbContext dbContext, IStringLocalizer<ValidationMessages> localizer, IUserContext userContext)
		{
			_dbContext = dbContext;
			_localizer = localizer;
			_userContext = userContext;
		}

		// Add a new product to an existing budget
		public async Task<BudgetProduct> AddProductToBudgetAsync(BudgetProduct budgetProduct)
		{
			// Validate ownership of the budget
			await ValidateBudgetOwnershipAsync(budgetProduct.BudgetID);

			// Validate product details
			ValidateBudgetProduct(budgetProduct);

			budgetProduct.CreatedAt = DateTime.UtcNow;
			budgetProduct.UpdatedAt = DateTime.UtcNow;

			_dbContext.BudgetProducts.Add(budgetProduct);
			await _dbContext.SaveChangesAsync();
			return budgetProduct;
		}

		// Remove a product from a budget
		public async Task<bool> RemoveProductFromBudgetAsync(int budgetProductId)
		{
			var budgetProduct = await _dbContext.BudgetProducts
				.Include(bp => bp.Budget)
				.ThenInclude(b => b.Contact)
				.FirstOrDefaultAsync(bp => bp.BudgetProductID == budgetProductId &&
										   bp.Budget.Contact.UserID == _userContext.UserId);

			if (budgetProduct == null)
				return false;

			_dbContext.BudgetProducts.Remove(budgetProduct);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		// Validation for product details
		private void ValidateBudgetProduct(BudgetProduct budgetProduct)
		{
			// Currency Validation
			var allowedCurrencies = new[] { "USD", "ARS" };
			if (!allowedCurrencies.Contains(budgetProduct.Currency))
				throw new InvalidOperationException(_localizer["InvalidCurrency"]);

			// Price Validation
			if (budgetProduct.BasePrice.HasValue && budgetProduct.BasePrice.Value < 0)
				throw new InvalidOperationException(_localizer["BasePricePositive"]);

			if (budgetProduct.FinalPrice <= 0)
				throw new InvalidOperationException(_localizer["FinalPriceRequired"]);

			if (budgetProduct.BasePrice.HasValue && budgetProduct.FinalPrice < budgetProduct.BasePrice.Value)
				throw new InvalidOperationException(_localizer["FinalPriceGreaterThanBasePrice"]);

			// Date Validation
			if (budgetProduct.CheckinDate.HasValue && budgetProduct.CheckoutDate.HasValue &&
				budgetProduct.CheckinDate >= budgetProduct.CheckoutDate)
				throw new InvalidOperationException(_localizer["InvalidDates"]);

			if (budgetProduct.ExpirationDate.HasValue && budgetProduct.BookingDate.HasValue &&
				budgetProduct.ExpirationDate <= budgetProduct.BookingDate)
				throw new InvalidOperationException(_localizer["ExpirationAfterBooking"]);
		}

		// Validate that the budget belongs to the logged-in user
		private async Task ValidateBudgetOwnershipAsync(int budgetId)
		{
			var userId = _userContext.UserId;

			var budget = await _dbContext.Budgets
				.Include(b => b.Contact)
				.FirstOrDefaultAsync(b => b.BudgetID == budgetId && b.Contact.UserID == userId);

			if (budget == null)
				throw new InvalidOperationException(_localizer["UnauthorizedBudgetAccess"]);
		}
	}
}
