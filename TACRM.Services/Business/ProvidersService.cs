using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface IProvidersService
	{
		Task<IEnumerable<Provider>> GetAllProvidersAsync();
		Task<Provider> GetProviderByIdAsync(int id);
		Task<Provider> CreateProviderAsync(Provider provider);
		Task<Provider> UpdateProviderAsync(Provider provider);
		Task<bool> DeleteProviderAsync(int id);
	}

	public class ProvidersService : IProvidersService
	{
		private readonly TACRMDbContext _dbContext;

		public ProvidersService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Provider>> GetAllProvidersAsync()
		{
			return await _dbContext.Providers.ToListAsync();
		}

		public async Task<Provider> GetProviderByIdAsync(int id)
		{
			return await _dbContext.Providers.FindAsync(id);
		}

		public async Task<Provider> CreateProviderAsync(Provider provider)
		{
			_dbContext.Providers.Add(provider);
			await _dbContext.SaveChangesAsync();
			return provider;
		}

		public async Task<Provider> UpdateProviderAsync(Provider provider)
		{
			var existingProvider = await _dbContext.Providers.FindAsync(provider.ProviderID);
			if (existingProvider == null) throw new KeyNotFoundException("Provider not found");

			_dbContext.Entry(existingProvider).CurrentValues.SetValues(provider);
			await _dbContext.SaveChangesAsync();
			return existingProvider;
		}

		public async Task<bool> DeleteProviderAsync(int id)
		{
			var provider = await _dbContext.Providers.FindAsync(id);
			if (provider == null) return false;

			_dbContext.Providers.Remove(provider);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
