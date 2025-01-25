using Microsoft.AspNetCore.Mvc;
using TACRM.Services.Core;
using TACRM.Services.Entities;

namespace TACRM.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CalendarEventsController : ControllerBase
	{
		private readonly ICalendarEventsService _calendarEventsService;

		public CalendarEventsController(ICalendarEventsService calendarEventsService)
		{
			_calendarEventsService = calendarEventsService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCalendarEvents()
		{
			var events = await _calendarEventsService.GetAllCalendarEventsAsync();
			return Ok(events);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCalendarEventById(int id)
		{
			var calendarEvent = await _calendarEventsService.GetCalendarEventByIdAsync(id);
			if (calendarEvent == null) return NotFound();

			return Ok(calendarEvent);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCalendarEvent([FromBody] CalendarEvent calendarEvent)
		{
			var createdEvent = await _calendarEventsService.CreateCalendarEventAsync(calendarEvent);
			return CreatedAtAction(nameof(GetCalendarEventById), new { id = createdEvent.EventID }, createdEvent);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCalendarEvent(int id, [FromBody] CalendarEvent calendarEvent)
		{
			if (id != calendarEvent.EventID) return BadRequest();

			try
			{
				var updatedEvent = await _calendarEventsService.UpdateCalendarEventAsync(calendarEvent);
				return Ok(updatedEvent);
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCalendarEvent(int id)
		{
			var success = await _calendarEventsService.DeleteCalendarEventAsync(id);
			if (!success) return NotFound();

			return NoContent();
		}
	}
}
