using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Repositories;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    private readonly BookStoreDatabaseContext _dbContext;

    public ClientRepository(BookStoreDatabaseContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _dbContext.Clients.AsNoTracking().Include(c => c.RentedBooks).ThenInclude(b => b.RentalHistory).ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _dbContext.Clients.AsNoTracking().Include(c => c.RentedBooks).ThenInclude(b => b.RentalHistory).FirstOrDefaultAsync(b => b.Id == id);
    }
}