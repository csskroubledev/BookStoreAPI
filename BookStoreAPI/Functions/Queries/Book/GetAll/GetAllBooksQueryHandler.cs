using AutoMapper;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries.Handlers;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _unitOfWork.Books.GetAllAsync();

        var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);
        return booksDto;
    }
}