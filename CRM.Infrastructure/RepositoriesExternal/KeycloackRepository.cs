using CRM.Domain;
using CRM.Domain.Models.External;
using CRM.Infrastructure.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace CRM.Infrastructure.RepositoriesExternal
{
    public class KeycloackRepository : IKeycloackRepository
    {
        public async Task<UsuarioKeycloack?> GetUsuarioKeycloackAsync(string id, AppConfiguration _configutation)
        {
            string url = $"{_configutation.AuthServerUrl}/admin/realms/{_configutation.Realm}/users/{id}";

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                TokenKeycloak token = await GetClientTokenAsync(_configutation);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<UsuarioKeycloack>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<TokenKeycloak?> GetClientTokenAsync(AppConfiguration _configutation)
        {
            string urlParaObterToken = _configutation.AuthServerUrl + "realms/" + _configutation.Realm + "/protocol/openid-connect/token";

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
                new KeyValuePair<string, string>( "client_id", _configutation.Resource),
                new KeyValuePair<string, string> ( "client_secret", _configutation.CredentialSecret )
            };

            var content = new FormUrlEncodedContent(pairs);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(urlParaObterToken, content);
                return JsonConvert.DeserializeObject<TokenKeycloak>(response.Content.ReadAsStringAsync().Result);
            }

        }
    }
}
