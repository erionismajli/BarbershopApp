using System.Text.Json.Serialization;

namespace Barbershop.Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    
    [JsonPropertyName("clientName")]
    public string ClientName { get; set; } = string.Empty;
    
    [JsonPropertyName("clientPhone")]
    public string ClientPhone { get; set; } = string.Empty;
    
    [JsonPropertyName("serviceType")]
    public string ServiceType { get; set; } = string.Empty;
    
    [JsonPropertyName("barberName")]
    public string BarberName { get; set; } = string.Empty;
    
    public string? Notes { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}

public class CreateBookingDto
{
    public string Date { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string BarberName { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateBookingDto
{
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? ClientName { get; set; }
    public string? ClientPhone { get; set; }
    public string? ServiceType { get; set; }
    public string? BarberName { get; set; }
    public string? Notes { get; set; }
}

public class BookingStatsDto
{
    public int TotalCount { get; set; }
    public int TodayCount { get; set; }
    public int WeekCount { get; set; }
}
