using CRM.Domain.Entities;
using CRM.Domain.Models.Cliente;

namespace CRM.Infrastructure.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<IEnumerable<Cliente>> GetFilteredAsync(ClienteFilterParams filterParams);
    }
}
