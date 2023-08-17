namespace BookStoreAPI.Interfaces;

public interface IUnitOfWork
{
    IBookRepository Books { get; }
    IClientRepository Clients { get; }

    Task<int> SaveAsync();
}