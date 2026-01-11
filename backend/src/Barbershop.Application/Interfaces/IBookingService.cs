using Barbershop.Application.DTOs;

namespace Barbershop.Application.Interfaces;

public interface IBookingService
{
    Task<ServiceResult<BookingDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<IEnumerable<BookingDto>>> GetAllAsync();
    Task<ServiceResult<BookingDto>> CreateAsync(CreateBookingDto dto);
    Task<ServiceResult<BookingDto>> UpdateAsync(Guid id, UpdateBookingDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<BookingStatsDto>> GetStatsAsync();
}

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ServiceResult<T> Success(T data, string? message = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static ServiceResult<T> Failure(string message, List<string>? errors = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
