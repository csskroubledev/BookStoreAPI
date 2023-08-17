using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;

namespace BookStoreAPI.Services;

public class ClientService : IClientService
{
    private IUnitOfWork _unitOfWork;
    
    public ClientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await _unitOfWork.Clients.GetAllAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _unitOfWork.Clients.GetByIdAsync(id);
    }

    public async Task<int> GetClientCountAsync()
    {
        return await _unitOfWork.Clients.GetCountAsync();
    }

    public async Task AddClientAsync(Client entity)
    {
        await _unitOfWork.Clients.AddAsync(entity);

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteClientAsync(int id)
    {
        var clientToRemove = await _unitOfWork.Clients.GetByIdAsync(id);
        ApiExceptionHandler.ThrowIf(clientToRemove is null, 404, "Client with specified ID doesn't exist.");
        
        _unitOfWork.Clients.Delete(clientToRemove);
        
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateClientAsync(int id, Client data)
    {
        var clientToEdit = await _unitOfWork.Clients.GetByIdAsync(id);
        ApiExceptionHandler.ThrowIf(clientToEdit is null, 404, "Client with specified ID doesn't exist.");
        
        _unitOfWork.Clients.Update(clientToEdit);
        
        clientToEdit.FirstName = data.FirstName;
        clientToEdit.LastName = data.LastName;
        clientToEdit.DateOfBirth = data.DateOfBirth;
        clientToEdit.RentedBooks = data.RentedBooks;

        await _unitOfWork.SaveAsync();
    }

    public async Task PatchClientAsync(int id, ClientPatchDto data)
    {
        var clientToEdit = await _unitOfWork.Clients.GetByIdAsync(id);
        ApiExceptionHandler.ThrowIf(clientToEdit is null, 404, "Client with specified ID doesn't exist.");
        
        _unitOfWork.Clients.Update(clientToEdit);
        
        clientToEdit.FirstName = data.FirstName ?? clientToEdit.FirstName;
        clientToEdit.LastName = data.LastName ?? clientToEdit.LastName;
        clientToEdit.DateOfBirth = data.DateOfBirth ?? clientToEdit.DateOfBirth;

        await _unitOfWork.SaveAsync();
    }
}