using CRM.Domain.Entities;
using CRM.Domain.Models.Cliente;
using CRM.Infrastructure.Context;
using CRM.Infrastructure.Interfaces;

namespace CRM.Infrastructure.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Cliente> clientes, int totalCount)> GetFilteredAsync(ClienteFilterParams filterParams)
        {
            return await GetFilteredAsync(filterParams, GetByFilterParameters);
        }

        private IQueryable<Cliente> GetByFilterParameters(IQueryable<Cliente> query, ClienteFilterParams filterParams)
        {
            query = query.OrderByDescending(c => c.DataCriacao);

            if (!string.IsNullOrEmpty(filterParams.Nome))
            {
                query = query.Where(x => x.Nome.Contains(filterParams.Nome));
            }

            if (!string.IsNullOrEmpty(filterParams.Email))
            {
                query = query.Where(x => x.Email.Contains(filterParams.Email));
            }

            if (!string.IsNullOrEmpty(filterParams.Telefone))
            {
                query = query.Where(x => x.Telefone.Contains(filterParams.Telefone));
            }

            if (!string.IsNullOrEmpty(filterParams.UsuarioCriacao))
            {
                query = query.Where(x => x.UsuarioCriacao.Contains(filterParams.UsuarioCriacao));
            }

            if (filterParams.DataCriacaoInicio.HasValue)
            {
                DateTime startDate = filterParams.DataCriacaoInicio.Value.Date.ToUniversalTime();
                query = query.Where(x => x.DataCriacao >= startDate);
            }

            if (filterParams.DataCriacaoFim.HasValue)
            {
                DateTime endDate = filterParams.DataCriacaoFim.Value.Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
                query = query.Where(x => x.DataCriacao <= endDate);
            }

            return query;
        }
    }
}
