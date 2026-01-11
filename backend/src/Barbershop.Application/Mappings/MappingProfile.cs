using AutoMapper;
using Barbershop.Application.DTOs;
using Barbershop.Domain.Entities;

namespace Barbershop.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Booking, BookingDto>();
        CreateMap<CreateBookingDto, Booking>();
        CreateMap<UpdateBookingDto, Booking>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
