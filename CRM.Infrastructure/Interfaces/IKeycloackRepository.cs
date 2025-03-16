using CRM.Domain;
using CRM.Domain.Models.External;

namespace CRM.Infrastructure.Interfaces
{
    public interface IKeycloackRepository
    {
        Task<UsuarioKeycloack?> GetUsuarioKeycloackAsync(string id, AppConfiguration _configutation);
    }
}
