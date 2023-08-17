using BookStoreAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookStoreAPI.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<int> GetBookCountAsync();
    Task AddBookAsync(Book entity);
    Task DeleteBookAsync(int id);
    Task UpdateBookAsync(int id, Book entity);
    Task PatchBookAsync(int id, BookPatchDto data);
}