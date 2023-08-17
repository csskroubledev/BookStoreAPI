using AutoMapper;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreDto, Genre>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Genre, GenreDto>();
    }
}