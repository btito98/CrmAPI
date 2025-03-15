using AutoMapper;
using CRM.Application.DTOs.Cliente;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Models.Cliente;
using CRM.Infrastructure.Interfaces;

namespace CRM.Application.Services
{
    public class ClienteService : BaseService<ClienteCreateDTO, Cliente, ClienteResultDTO>, IClienteService
    {
        protected readonly IMapper _mapper;
        protected readonly IClienteRepository clienteRepository;
        public ClienteService(IBaseRepository<Cliente> repository, IClienteRepository clienteRepository, IMapper mapper) : base(repository, mapper)
        {
            this.clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public override async Task<ClienteResultDTO> AddAsync(ClienteCreateDTO dto)
        {
            return await base.AddAsync(dto);
        }

        public async Task<IEnumerable<ClienteResultDTO>> GetFilteredAsync(ClienteFilterParams filterParams)
        {
            var entities = await clienteRepository.GetFilteredAsync(filterParams);

            return _mapper.Map<IEnumerable<ClienteResultDTO>>(entities);
        }
    }
}
