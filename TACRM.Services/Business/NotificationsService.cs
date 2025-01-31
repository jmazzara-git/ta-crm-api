using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface INotificationsService
	{
		Task<IEnumerable<Notification>> GetNotificationsAsync();
		Task<Notification> GetNotificationByIdAsync(int id);
		Task<Notification> CreateNotificationAsync(Notification notification);
		Task<bool> MarkAsReadAsync(int id);
		Task<bool> DeleteNotificationAsync(int id);
	}

	public class NotificationsService : INotificationsService
	{
		private readonly TACRMDbContext _dbContext;
		private readonly IUserContext _userContext;

		public NotificationsService(TACRMDbContext dbContext, IUserContext userContext)
		{
			_dbContext = dbContext;
			_userContext = userContext;
		}

		public async Task<IEnumerable<Notification>> GetNotificationsAsync()
		{
			var userId = _userContext.GetUserId();
			return await _dbContext.Notifications
				.Where(n => n.UserID == userId)
				.OrderByDescending(n => n.CreatedAt)
				.ToListAsync();
		}

		public async Task<Notification> GetNotificationByIdAsync(int id)
		{
			var userId = _userContext.GetUserId();
			return await _dbContext.Notifications
				.FirstOrDefaultAsync(n => n.NotificationID == id && n.UserID == userId);
		}

		public async Task<Notification> CreateNotificationAsync(Notification notification)
		{
			_dbContext.Notifications.Add(notification);
			await _dbContext.SaveChangesAsync();
			return notification;
		}

		public async Task<bool> MarkAsReadAsync(int id)
		{
			var notification = await _dbContext.Notifications.FindAsync(id);
			if (notification == null || notification.UserID != _userContext.GetUserId())
				return false;

			notification.IsRead = true;
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteNotificationAsync(int id)
		{
			var notification = await _dbContext.Notifications.FindAsync(id);
			if (notification == null || notification.UserID != _userContext.GetUserId())
				return false;

			_dbContext.Notifications.Remove(notification);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
