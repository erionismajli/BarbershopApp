using Microsoft.EntityFrameworkCore;
using Barbershop.Domain.Entities;
using Barbershop.Domain.Interfaces;
using Barbershop.Infrastructure.Data;

namespace Barbershop.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BarbershopDbContext _context;

    public BookingRepository(BarbershopDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<Booking> AddAsync(Booking entity)
    {
        await _context.Bookings.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Bookings.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Bookings.AnyAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Bookings
            .Where(b => DateTime.Parse(b.Date) >= startDate && DateTime.Parse(b.Date) <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByBarberAsync(string barberName)
    {
        return await _context.Bookings
            .Where(b => b.BarberName == barberName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByDateAsync(string date)
    {
        return await _context.Bookings
            .Where(b => b.Date == date)
            .ToListAsync();
    }

    public async Task<BookingStats> GetStatsAsync()
    {
        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var now = DateTime.Now;
        var weekStart = now.AddDays(-(int)now.DayOfWeek);
        weekStart = new DateTime(weekStart.Year, weekStart.Month, weekStart.Day, 0, 0, 0);
        var weekEnd = weekStart.AddDays(7);

        var allBookings = await _context.Bookings.ToListAsync();

        var stats = new BookingStats
        {
            TotalCount = allBookings.Count,
            TodayCount = allBookings.Count(b => b.Date == today),
            WeekCount = allBookings.Count(b =>
            {
                if (DateTime.TryParse(b.Date, out var bookingDate))
                {
                    return bookingDate >= weekStart && bookingDate < weekEnd;
                }
                return false;
            })
        };

        return stats;
    }
}
