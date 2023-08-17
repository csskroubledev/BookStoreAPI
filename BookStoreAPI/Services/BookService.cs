using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreAPI.Services;

public class BookService : IBookService
{
    private IUnitOfWork _unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _unitOfWork.Books.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _unitOfWork.Books.GetByIdAsync(id);
    }

    public async Task<int> GetBookCountAsync()
    {
        return await _unitOfWork.Books.GetCountAsync();
    }

    public async Task AddBookAsync(Book entity)
    {
        await _unitOfWork.Books.AddAsync(entity);

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var bookToRemove = await _unitOfWork.Books.GetByIdAsync(id);
        ApiExceptionHandler.ThrowIf(bookToRemove is null, 404, "Book with specified ID doesn't exist.");
        
        _unitOfWork.Books.Delete(bookToRemove);

        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateBookAsync(int id, Book book)
    {
        var bookToEdit = await _unitOfWork.Books.GetByIdAsync(id);
        ApiExceptionHandler.ThrowIf(bookToEdit is null, 404, "Book with specified ID doesn't exist.");

        bookToEdit.RentDate = book.RentDate;
        if (book.ClientId is not null && bookToEdit.ClientId != book.ClientId)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(book.ClientId.Value);
            ApiExceptionHandler.ThrowIf(client is null, 404, "Client with specified ID doesn't exist.");

            bookToEdit.ClientId = book.ClientId;
            if (book.RentDate is null)
            {
                bookToEdit.RentDate = DateTime.Now;
            }
        }
        
        _unitOfWork.Books.Update(bookToEdit);
        
        bookToEdit.Title = book.Title;
        bookToEdit.Author = book.Author;
        bookToEdit.Price = book.Price;
        bookToEdit.ReleaseDate = book.ReleaseDate;
        bookToEdit.GenreId = book.GenreId;

        await _unitOfWork.SaveAsync();
    }

    public async Task PatchBookAsync(int id, BookPatchDto data)
    {
        var bookToEdit = await _unitOfWork.Books.GetByIdAsync(id);
        ApiExceptionHandler.ThrowIf(bookToEdit is null, 404, "Book with specified ID doesn't exist.");
        
        _unitOfWork.Books.Update(bookToEdit);
        
        if (data.ClientId is not null && bookToEdit.ClientId != data.ClientId)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(data.ClientId.Value);
            ApiExceptionHandler.ThrowIf(client is null, 404, "Client with specified ID doesn't exist.");

            bookToEdit.ClientId = data.ClientId;
            if (data.RentDate is null)
            {
                bookToEdit.RentDate = DateTime.Now;
            }
        }
        bookToEdit.RentDate = data.RentDate ?? bookToEdit.RentDate;
        bookToEdit.Title = data.Title ?? bookToEdit.Title;
        bookToEdit.Author = data.Author ?? bookToEdit.Author;
        bookToEdit.ReleaseDate = data.ReleaseDate ?? bookToEdit.ReleaseDate;
        bookToEdit.Price = data.Price ?? bookToEdit.Price;
        bookToEdit.GenreId = data.GenreId ?? bookToEdit.GenreId;

        await _unitOfWork.SaveAsync();
    }
}