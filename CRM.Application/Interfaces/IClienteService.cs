using CRM.Application.DTOs.Cliente;
using CRM.Domain.Entities;

namespace CRM.Application.Interfaces
{
    public interface IClienteService : IBaseService<ClienteCreateDTO, Cliente, ClienteResultDTO>
    {
    }
}
