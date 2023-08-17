using AutoMapper;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<BookDto, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));
    }
}