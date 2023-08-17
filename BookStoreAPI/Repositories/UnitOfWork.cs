using BookStoreAPI.Interfaces;

namespace BookStoreAPI.Models.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookStoreDatabaseContext _dbContext;

    public UnitOfWork(BookStoreDatabaseContext dbContext, IBookRepository booksRepository, IClientRepository clients)
    {
        _dbContext = dbContext;
        Books = booksRepository;
        Clients = clients;
    }

    public IBookRepository Books { get; }
    public IClientRepository Clients { get; }

    public async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) _dbContext.Dispose();
    }
}