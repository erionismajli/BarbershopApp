using Microsoft.AspNetCore.Mvc;
using Barbershop.Application.DTOs;
using Barbershop.Application.Interfaces;

namespace Barbershop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookingDto>>>> GetAll()
    {
        try
        {
            var result = await _bookingService.GetAllAsync();
            
            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<IEnumerable<BookingDto>>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(new ApiResponse<IEnumerable<BookingDto>>
            {
                IsSuccess = true,
                Data = result.Data,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings");
            return StatusCode(500, new ApiResponse<IEnumerable<BookingDto>>
            {
                IsSuccess = false,
                Message = "An error occurred while retrieving bookings"
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _bookingService.GetByIdAsync(id);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ApiResponse<BookingDto>
                {
                    IsSuccess = false,
                    Message = result.Message
                });
            }

            return Ok(new ApiResponse<BookingDto>
            {
                IsSuccess = true,
                Data = result.Data,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking {BookingId}", id);
            return StatusCode(500, new ApiResponse<BookingDto>
            {
                IsSuccess = false,
                Message = "An error occurred while retrieving the booking"
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Create([FromBody] CreateBookingDto dto)
    {
        try
        {
            var result = await _bookingService.CreateAsync(dto);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<BookingDto>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, new ApiResponse<BookingDto>
            {
                IsSuccess = true,
                Data = result.Data,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return StatusCode(500, new ApiResponse<BookingDto>
            {
                IsSuccess = false,
                Message = "An error occurred while creating the booking"
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Update(Guid id, [FromBody] UpdateBookingDto dto)
    {
        try
        {
            var result = await _bookingService.UpdateAsync(id, dto);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ApiResponse<BookingDto>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(new ApiResponse<BookingDto>
            {
                IsSuccess = true,
                Data = result.Data,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking {BookingId}", id);
            return StatusCode(500, new ApiResponse<BookingDto>
            {
                IsSuccess = false,
                Message = "An error occurred while updating the booking"
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        try
        {
            var result = await _bookingService.DeleteAsync(id);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = result.Message
                });
            }

            return Ok(new ApiResponse<bool>
            {
                IsSuccess = true,
                Data = result.Data,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking {BookingId}", id);
            return StatusCode(500, new ApiResponse<bool>
            {
                IsSuccess = false,
                Message = "An error occurred while deleting the booking"
            });
        }
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<BookingStatsDto>>> GetStats()
    {
        try
        {
            var result = await _bookingService.GetStatsAsync();
            
            if (!result.IsSuccess)
            {
                return BadRequest(new ApiResponse<BookingStatsDto>
                {
                    IsSuccess = false,
                    Message = result.Message
                });
            }

            return Ok(new ApiResponse<BookingStatsDto>
            {
                IsSuccess = true,
                Data = result.Data,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stats");
            return StatusCode(500, new ApiResponse<BookingStatsDto>
            {
                IsSuccess = false,
                Message = "An error occurred while retrieving stats"
            });
        }
    }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
}
