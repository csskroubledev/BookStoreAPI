using AutoMapper;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using MediatR;

namespace BookStoreAPI.Queries.Handlers;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
{
    private IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public GetBookQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
        ApiExceptionHandler.ThrowIf(book is null, 404, $"Book with ID {request.BookId} doesn't exist.");
        
        var bookDto = _mapper.Map<BookDto>(book);
        
        return bookDto;
    }
}