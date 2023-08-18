using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    private readonly BookStoreDatabaseContext _dbContext;

    public BookRepository(BookStoreDatabaseContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbContext.Books.Include(b => b.RentalHistory).Include(b => b.Genre).AsNoTracking().ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _dbContext.Books.Include(b => b.RentalHistory).Include(b => b.Genre).AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}