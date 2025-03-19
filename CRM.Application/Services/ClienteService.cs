using AutoMapper;
using CRM.Application.DTOs.Cliente;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Models.Cliente;
using CRM.Infrastructure.Interfaces;
using CRM.Shared.Results;

namespace CRM.Application.Services
{
    public class ClienteService : BaseService<ClienteCreateDTO, Cliente, ClienteResultDTO>, IClienteService
    {
        protected readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository repository, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork, mapper)
        {
            _clienteRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result<(IEnumerable<ClienteResultDTO> clientes, int totalCount)>> GetFilteredAsync(ClienteFilterParams filterParams)
        {
            if (filterParams == null)
                return Result<(IEnumerable<ClienteResultDTO>, int)>.Failure(
                    new Error("ClienteService.GetFilteredAsync", "Parâmetros de filtro inválidos.")
                );

            var (entities, totalCount) = await _clienteRepository.GetFilteredAsync(filterParams);

            if (!entities.Any())
                return Result<(IEnumerable<ClienteResultDTO>, int)>.Failure(
                    new Error("ClienteService.GetFilteredAsync", "Nenhum cliente encontrado com os filtros informados.")
                );

            return Result<(IEnumerable<ClienteResultDTO>, int)>.Success(
                (_mapper.Map<IEnumerable<ClienteResultDTO>>(entities), totalCount)
            );
        }
    }
}