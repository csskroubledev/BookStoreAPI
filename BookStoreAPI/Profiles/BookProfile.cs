using AutoMapper;
using BookStoreAPI.Commands;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<BookDto, Book>()
            .ForMember(dest => dest.RentalHistory, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));

        CreateMap<UpdateBookCommand, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}