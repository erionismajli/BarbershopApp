using Barbershop.Domain.Entities;

namespace Barbershop.Domain.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Booking>> GetByBarberAsync(string barberName);
    Task<IEnumerable<Booking>> GetByDateAsync(string date);
    Task<BookingStats> GetStatsAsync();
}

public class BookingStats
{
    public int TotalCount { get; set; }
    public int TodayCount { get; set; }
    public int WeekCount { get; set; }
}
