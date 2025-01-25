using Microsoft.EntityFrameworkCore;
using TACRM.Services.Entities;

namespace TACRM.Services.Core
{
	public interface ICalendarEventsService
	{
		Task<IEnumerable<CalendarEvent>> GetAllCalendarEventsAsync();
		Task<CalendarEvent> GetCalendarEventByIdAsync(int id);
		Task<CalendarEvent> CreateCalendarEventAsync(CalendarEvent calendarEvent);
		Task<CalendarEvent> UpdateCalendarEventAsync(CalendarEvent calendarEvent);
		Task<bool> DeleteCalendarEventAsync(int id);
	}

	public class CalendarEventsService : ICalendarEventsService
	{
		private readonly TACRMDbContext _dbContext;

		public CalendarEventsService(TACRMDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<CalendarEvent>> GetAllCalendarEventsAsync()
		{
			return await _dbContext.CalendarEvents.ToListAsync();
		}

		public async Task<CalendarEvent> GetCalendarEventByIdAsync(int id)
		{
			return await _dbContext.CalendarEvents.FindAsync(id);
		}

		public async Task<CalendarEvent> CreateCalendarEventAsync(CalendarEvent calendarEvent)
		{
			_dbContext.CalendarEvents.Add(calendarEvent);
			await _dbContext.SaveChangesAsync();
			return calendarEvent;
		}

		public async Task<CalendarEvent> UpdateCalendarEventAsync(CalendarEvent calendarEvent)
		{
			var existingEvent = await _dbContext.CalendarEvents.FindAsync(calendarEvent.EventID);
			if (existingEvent == null) throw new KeyNotFoundException("Calendar event not found");

			_dbContext.Entry(existingEvent).CurrentValues.SetValues(calendarEvent);
			await _dbContext.SaveChangesAsync();
			return existingEvent;
		}

		public async Task<bool> DeleteCalendarEventAsync(int id)
		{
			var calendarEvent = await _dbContext.CalendarEvents.FindAsync(id);
			if (calendarEvent == null) return false;

			_dbContext.CalendarEvents.Remove(calendarEvent);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
