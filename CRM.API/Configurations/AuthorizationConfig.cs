using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk;
using Microsoft.AspNetCore.Authorization;

namespace CRM.API.Configurations
{
    public class AuthorizationConfig
    {
        public static void AddAdmin(IConfiguration configuration, IServiceCollection service)
        {
            var keycloakOptions = configuration.GetSection("KeycloakAdminOptions").Get<KeycloakAdminClientOptions>();

            service.Configure<KeycloakAdminClientOptions>(configuration.GetSection("KeycloakAdminOptions"));

            service.AddSingleton(keycloakOptions);

            service.AddDistributedMemoryCache();
            service
                .AddClientCredentialsTokenManagement()
                .AddClient(
                    keycloakOptions.Resource,
                    client =>
                    {
                        client.ClientId = keycloakOptions.Resource;
                        client.ClientSecret = keycloakOptions.Credentials.Secret;
                        client.TokenEndpoint = keycloakOptions.KeycloakTokenEndpoint;
                    }
                );

            service
                .AddKeycloakAdminHttpClient(configuration)
                .AddClientCredentialsTokenHandler(keycloakOptions.Resource);
        }

        public static void AddAuthentication(IConfiguration configuration, IServiceCollection service)
        {
            service.AddKeycloakWebApiAuthentication(configuration);
        }

        public static void AddAuthorization(IConfiguration configuration, IServiceCollection service)
        {
            service.AddAuthorization(options =>
            {
                ConfigureClientePolicies(options);
            }).AddKeycloakAuthorization(configuration);
        }

        private static void ConfigureClientePolicies(AuthorizationOptions options)
        {
            options.AddPolicy("ReadCliente", policy =>
            {
                policy.RequireResourceRoles("read_cliente");
            });

            options.AddPolicy("CreateCliente", policy =>
            {
                policy.RequireResourceRoles("create_cliente");
            });
        }
    }
}
