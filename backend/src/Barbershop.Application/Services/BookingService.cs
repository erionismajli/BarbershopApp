using AutoMapper;
using Barbershop.Application.DTOs;
using Barbershop.Application.Interfaces;
using Barbershop.Domain.Entities;
using Barbershop.Domain.Interfaces;
using FluentValidation;

namespace Barbershop.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateBookingDto> _createValidator;

    public BookingService(
        IBookingRepository repository,
        IMapper mapper,
        IValidator<CreateBookingDto> createValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<ServiceResult<BookingDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
            {
                return ServiceResult<BookingDto>.Failure("Booking not found");
            }

            var dto = _mapper.Map<BookingDto>(booking);
            return ServiceResult<BookingDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return ServiceResult<BookingDto>.Failure($"Error retrieving booking: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<BookingDto>>> GetAllAsync()
    {
        try
        {
            var bookings = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return ServiceResult<IEnumerable<BookingDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<BookingDto>>.Failure($"Error retrieving bookings: {ex.Message}");
        }
    }

    public async Task<ServiceResult<BookingDto>> CreateAsync(CreateBookingDto dto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return ServiceResult<BookingDto>.Failure("Validation failed", errors);
            }

            var booking = _mapper.Map<Booking>(dto);
            var created = await _repository.AddAsync(booking);
            var createdDto = _mapper.Map<BookingDto>(created);

            return ServiceResult<BookingDto>.Success(createdDto, "Booking created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<BookingDto>.Failure($"Error creating booking: {ex.Message}");
        }
    }

    public async Task<ServiceResult<BookingDto>> UpdateAsync(Guid id, UpdateBookingDto dto)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return ServiceResult<BookingDto>.Failure("Booking not found");
            }

            if (!string.IsNullOrEmpty(dto.Date)) existing.Date = dto.Date;
            if (!string.IsNullOrEmpty(dto.Time)) existing.Time = dto.Time;
            if (!string.IsNullOrEmpty(dto.ClientName)) existing.ClientName = dto.ClientName;
            if (!string.IsNullOrEmpty(dto.ClientPhone)) existing.ClientPhone = dto.ClientPhone;
            if (!string.IsNullOrEmpty(dto.ServiceType)) existing.ServiceType = dto.ServiceType;
            if (!string.IsNullOrEmpty(dto.BarberName)) existing.BarberName = dto.BarberName;
            if (dto.Notes != null) existing.Notes = dto.Notes;

            await _repository.UpdateAsync(existing);
            var updatedDto = _mapper.Map<BookingDto>(existing);

            return ServiceResult<BookingDto>.Success(updatedDto, "Booking updated successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<BookingDto>.Failure($"Error updating booking: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return ServiceResult<bool>.Failure("Booking not found");
            }

            await _repository.DeleteAsync(id);
            return ServiceResult<bool>.Success(true, "Booking deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Error deleting booking: {ex.Message}");
        }
    }

    public async Task<ServiceResult<BookingStatsDto>> GetStatsAsync()
    {
        try
        {
            var stats = await _repository.GetStatsAsync();
            var dto = new BookingStatsDto
            {
                TotalCount = stats.TotalCount,
                TodayCount = stats.TodayCount,
                WeekCount = stats.WeekCount
            };

            return ServiceResult<BookingStatsDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return ServiceResult<BookingStatsDto>.Failure($"Error retrieving stats: {ex.Message}");
        }
    }
}
