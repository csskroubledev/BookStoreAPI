using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientService _clientService;

        public ClientController(IMapper mapper, IClientService clientService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _clientService = clientService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<ClientDto>> Get()
        {
            return _mapper.Map<IEnumerable<ClientDto>>(await _clientService.GetAllClientsAsync());
        }

        [HttpGet("{id}", Name = "GetClientById")]
        public async Task<ClientDto> Get(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            ApiExceptionHandler.ThrowIf(client is null, 404, "Client with specified ID doesn't exist.");

            return _mapper.Map<ClientDto>(client);
        }

        [HttpPost]
        public async Task Post([FromBody] ClientDto data)
        {
            var client = _mapper.Map<Client>(data);
            await _clientService.AddClientAsync(client);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] ClientDto data)
        {
            var client = _mapper.Map<Client>(data);
            await _clientService.UpdateClientAsync(id, client);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _clientService.DeleteClientAsync(id);
        }

        [HttpPatch("{id}")]
        public async Task Patch(int id, [FromBody] ClientPatchDto data)
        {
            await _clientService.PatchClientAsync(id, data);
        }
    }
}
