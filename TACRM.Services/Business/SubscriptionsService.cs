using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface ISubscriptionsService
	{
		Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
		Task<Subscription> GetSubscriptionByIdAsync(int id);
		Task<Subscription> CreateSubscriptionAsync(Subscription subscription);
		Task<Subscription> UpdateSubscriptionAsync(Subscription subscription);
		Task<bool> DeleteSubscriptionAsync(int id);
	}

	public class SubscriptionsService : ISubscriptionsService
	{
		private readonly TACRMDbContext _dbContext;

		public SubscriptionsService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
		{
			return await _dbContext.Subscriptions.Include(s => s.User).ToListAsync();
		}

		public async Task<Subscription> GetSubscriptionByIdAsync(int id)
		{
			return await _dbContext.Subscriptions.Include(s => s.User).FirstOrDefaultAsync(s => s.SubscriptionID == id);
		}

		public async Task<Subscription> CreateSubscriptionAsync(Subscription subscription)
		{
			_dbContext.Subscriptions.Add(subscription);
			await _dbContext.SaveChangesAsync();
			return subscription;
		}

		public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
		{
			var existingSubscription = await _dbContext.Subscriptions.FindAsync(subscription.SubscriptionID);
			if (existingSubscription == null) throw new KeyNotFoundException("Subscription not found");

			_dbContext.Entry(existingSubscription).CurrentValues.SetValues(subscription);
			await _dbContext.SaveChangesAsync();
			return existingSubscription;
		}

		public async Task<bool> DeleteSubscriptionAsync(int id)
		{
			var subscription = await _dbContext.Subscriptions.FindAsync(id);
			if (subscription == null) return false;

			_dbContext.Subscriptions.Remove(subscription);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
