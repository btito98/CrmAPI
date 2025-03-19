using CRM.Application.DTOs.Cliente;
using CRM.Domain.Entities;
using CRM.Domain.Models.Cliente;
using CRM.Shared.Results;

namespace CRM.Application.Interfaces
{
    public interface IClienteService : IBaseService<ClienteCreateDTO, Cliente, ClienteResultDTO>
    {
        Task<Result<(IEnumerable<ClienteResultDTO> clientes, int totalCount)>> GetFilteredAsync(ClienteFilterParams filterParams);
    }
}