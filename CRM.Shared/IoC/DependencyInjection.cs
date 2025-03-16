using AutoMapper;
using CRM.Application.DTOs.Cliente;
using CRM.Application.Interfaces;
using CRM.Application.Mappings;
using CRM.Application.Services;
using CRM.Application.Validators.Cliente;
using CRM.Domain;
using CRM.Infrastructure.Context;
using CRM.Infrastructure.Interfaces;
using CRM.Infrastructure.Repositories;
using CRM.Infrastructure.RepositoriesExternal;
using FluentValidation;
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

            services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
            services.AddScoped<IClienteService, ClienteService>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IKeycloackRepository, KeycloackRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<AppConfiguration>((s) =>
            {
                return new AppConfiguration(
                    realm: configuration.GetSection("Keycloak").GetSection("realm").Value,
                    authServerUrl: configuration.GetSection("Keycloak").GetSection("auth-server-url").Value,
                    resource: configuration.GetSection("Keycloak").GetSection("resource").Value,
                    credentialSecret: configuration.GetSection("Keycloak").GetSection("credentials").GetSection("secret").Value);
            });

            services.AddScoped<IValidator<ClienteCreateDTO>, ClienteCreateDTOValidator>();

            services.AddSingleton(new MapperConfiguration(config => config.Configure()).CreateMapper());
        }
    }
}
