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
        protected readonly IClienteRepository _repository;
        public ClienteService(IClienteRepository repository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork, mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<(IEnumerable<ClienteResultDTO> clientes, int totalCount)> GetFilteredAsync(ClienteFilterParams filterParams)
        {
            var (entities, totalCount) = await _repository.GetFilteredAsync(filterParams);

            var clientes = _mapper.Map<IEnumerable<ClienteResultDTO>>(entities);

            return (clientes, totalCount);
        }         

    }
}
