using CRM.Application.DTOs.Cliente;
using CRM.Domain.Entities;
using CRM.Domain.Models.Cliente;

namespace CRM.Application.Interfaces
{
    public interface IClienteService : IBaseService<ClienteCreateDTO, Cliente, ClienteResultDTO>
    {
        Task<IEnumerable<ClienteResultDTO>> GetFilteredAsync(ClienteFilterParams filterParams);
    }
}
