using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class NotificationsController : ControllerBase
	{
		private readonly INotificationsService _notificationsService;

		public NotificationsController(INotificationsService notificationsService)
		{
			_notificationsService = notificationsService;
		}

		[HttpGet]
		public async Task<IActionResult> GetNotifications()
		{
			var notifications = await _notificationsService.GetNotificationsAsync();
			return Ok(notifications);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetNotificationById(int id)
		{
			var notification = await _notificationsService.GetNotificationByIdAsync(id);
			if (notification == null) return NotFound();

			return Ok(notification);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
		{
			var createdNotification = await _notificationsService.CreateNotificationAsync(notification);
			return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.NotificationID }, createdNotification);
		}

		[HttpPatch("{id}/mark-as-read")]
		public async Task<IActionResult> MarkAsRead(int id)
		{
			var success = await _notificationsService.MarkAsReadAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNotification(int id)
		{
			var success = await _notificationsService.DeleteNotificationAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
