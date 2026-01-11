using Microsoft.EntityFrameworkCore;
using Barbershop.Application.Interfaces;
using Barbershop.Application.Services;
using Barbershop.Application.Mappings;
using Barbershop.Application.Validators;
using Barbershop.Domain.Interfaces;
using Barbershop.Infrastructure.Data;
using Barbershop.Infrastructure.Repositories;
using FluentValidation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<BarbershopDbContext>(options =>
    options.UseInMemoryDatabase("BarbershopDb"));

// Repositories
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Services
builder.Services.AddScoped<IBookingService, BookingService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingDtoValidator>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BarbershopDbContext>();
    SeedData(context);
}

app.Run();

static void SeedData(BarbershopDbContext context)
{
    if (!context.Bookings.Any())
    {
        var bookings = new[]
        {
            new Barbershop.Domain.Entities.Booking
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"),
                Time = "10:00",
                ClientName = "John Doe",
                ClientPhone = "(555) 123-4567",
                ServiceType = "Classic Cut",
                BarberName = "Alex",
                Notes = "Regular haircut",
                CreatedAt = DateTime.UtcNow
            },
            new Barbershop.Domain.Entities.Booking
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
                Time = "14:30",
                ClientName = "Jane Smith",
                ClientPhone = "(555) 987-6543",
                ServiceType = "Beard Trim",
                BarberName = "Jordan",
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Bookings.AddRange(bookings);
        context.SaveChanges();
    }
}
