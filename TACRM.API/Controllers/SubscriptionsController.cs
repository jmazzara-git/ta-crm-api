using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SubscriptionsController : ControllerBase
	{
		private readonly ISubscriptionsService _subscriptionsService;

		public SubscriptionsController(ISubscriptionsService subscriptionsService)
		{
			_subscriptionsService = subscriptionsService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllSubscriptions()
		{
			var subscriptions = await _subscriptionsService.GetAllSubscriptionsAsync();
			return Ok(subscriptions);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetSubscriptionById(int id)
		{
			var subscription = await _subscriptionsService.GetSubscriptionByIdAsync(id);
			if (subscription == null) return NotFound();

			return Ok(subscription);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSubscription([FromBody] Subscription subscription)
		{
			var createdSubscription = await _subscriptionsService.CreateSubscriptionAsync(subscription);
			return CreatedAtAction(nameof(GetSubscriptionById), new { id = createdSubscription.SubscriptionID }, createdSubscription);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateSubscription(int id, [FromBody] Subscription subscription)
		{
			if (id != subscription.SubscriptionID) return BadRequest();

			try
			{
				var updatedSubscription = await _subscriptionsService.UpdateSubscriptionAsync(subscription);
				return Ok(updatedSubscription);
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSubscription(int id)
		{
			var success = await _subscriptionsService.DeleteSubscriptionAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
