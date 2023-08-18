using AutoMapper;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class RentalProfile : Profile
{
    public RentalProfile()
    {
        CreateMap<RentalHistoryDto, RentalHistory>()
            .ForMember(opt => opt.Id, opt => opt.Ignore());
        CreateMap<RentalHistory, RentalHistoryDto>();
    }
}