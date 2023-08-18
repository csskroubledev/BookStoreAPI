using AutoMapper;
using BookStoreAPI.Commands;
using BookStoreAPI.Models;

namespace BookStoreAPI.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<ClientDto, Client>()
            .ForMember(dest => dest.RentedBooks, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Client, ClientDto>();

        CreateMap<ClientPatchDto, Client>()
            .ForMember(dest => dest.RentedBooks, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Client, ClientPatchDto>();

        CreateMap<UpdateClientCommand, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}