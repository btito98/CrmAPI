using AutoMapper;
using CRM.Application.DTOs.Cliente;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Interfaces;

namespace CRM.Application.Services
{
    public class ClienteService : BaseService<ClienteCreateDTO, Cliente, ClienteResultDTO>, IClienteService
    {
        protected readonly IMapper _mapper;
        public ClienteService(IBaseRepository<Cliente> repository, IMapper mapper) : base(repository, mapper) {}

        public override async Task<ClienteResultDTO> AddAsync(ClienteCreateDTO dto)
        {
            return await base.AddAsync(dto);
        }
    }
}
