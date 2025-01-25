using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface IUsersService
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<User> CreateUserAsync(User user);
		Task<User> UpdateUserAsync(User user);
		Task<bool> DeleteUserAsync(int id);
	}

	public class UsersService : IUsersService
	{
		private readonly TACRMDbContext _dbContext;

		public UsersService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return await _dbContext.Users.Include(u => u.Agency).ToListAsync();
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _dbContext.Users.Include(u => u.Agency).FirstOrDefaultAsync(u => u.UserID == id);
		}

		public async Task<User> CreateUserAsync(User user)
		{
			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();
			return user;
		}

		public async Task<User> UpdateUserAsync(User user)
		{
			var existingUser = await _dbContext.Users.FindAsync(user.UserID);
			if (existingUser == null) throw new KeyNotFoundException("User not found");

			_dbContext.Entry(existingUser).CurrentValues.SetValues(user);
			await _dbContext.SaveChangesAsync();
			return existingUser;
		}

		public async Task<bool> DeleteUserAsync(int id)
		{
			var user = await _dbContext.Users.FindAsync(id);
			if (user == null) return false;

			_dbContext.Users.Remove(user);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
