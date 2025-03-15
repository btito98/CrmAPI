using CRM.Application.Mappings;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Shared.IoC
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null, empty, or whitespace.");

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
        }
    }
}
