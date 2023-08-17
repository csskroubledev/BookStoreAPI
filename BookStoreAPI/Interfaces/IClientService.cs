using Microsoft.AspNetCore.JsonPatch;
using BookStoreAPI.Models;

namespace BookStoreAPI.Interfaces;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task<Client?> GetClientByIdAsync(int id);
    Task<int> GetClientCountAsync();
    Task AddClientAsync(Client entity);
    Task DeleteClientAsync(int id);
    Task UpdateClientAsync(int id, Client entity);
    Task PatchClientAsync(int id, ClientPatchDto data);
}