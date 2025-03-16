using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;

namespace CRM.API.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private const string ClientIdKey = "client_id";
        private const string ClientSecretKey = "client_secret";
        private const string GrantTypeKey = "grant_type";
        private const string ScopeKey = "scope";
        private const string PasswordGrantType = "password";
        private const string OpenIdScope = "openid";
        private const string TokenEndpointPath = "/protocol/openid-connect/token";

        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenHandler> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public TokenHandler(IConfiguration configuration, ILogger<TokenHandler> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _clientId = _configuration["Keycloak:resource"];
            _clientSecret = _configuration["Keycloak:credentials:secret"];

            if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret))
            {
                throw new InvalidOperationException("Client ID or Client Secret is missing in configuration.");
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (IsTokenRequest(request))
                {
                    var requestData = await request.Content.ReadAsStringAsync();
                    var formData = BuildFormData(requestData);

                    request.Content = new FormUrlEncodedContent(formData);
                    var response = await base.SendAsync(request, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        return await HandleErrorResponseAsync(response);
                    }

                    return response;
                }

                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Invalid request path or method.", Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new { message = ex.Message }),
                        Encoding.UTF8,
                        "application/json"
                    )
                };
            }
        }

        private bool IsTokenRequest(HttpRequestMessage request)
        {
            return request.Method == HttpMethod.Post &&
                   request.RequestUri.AbsolutePath.Contains(TokenEndpointPath);
        }

        private Dictionary<string, string> BuildFormData(string requestData)
        {
            var formData = new Dictionary<string, string>
            {
                { ClientIdKey, _clientId },
                { ClientSecretKey, _clientSecret },
                { GrantTypeKey, PasswordGrantType },
                { ScopeKey, OpenIdScope }
            };

            if (!string.IsNullOrEmpty(requestData))
            {
                var parsedForm = HttpUtility.ParseQueryString(requestData);
                foreach (string key in parsedForm.AllKeys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        formData[key] = parsedForm[key];
                    }
                }
            }

            return formData;
        }

        private async Task<HttpResponseMessage> HandleErrorResponseAsync(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Keycloak error response: {ErrorContent}", errorContent);

            try
            {
                var errorMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorContent);
                if (errorMessage != null && errorMessage.ContainsKey("message"))
                {
                    var detailedError = JsonConvert.DeserializeObject<KeycloakErrorDetails>(errorMessage["message"]);
                    if (detailedError != null)
                    {
                        response.Content = new StringContent(
                            JsonConvert.SerializeObject(new
                            {
                                error = detailedError.Error,
                                errorDescription = detailedError.ErrorDescription
                            }),
                            Encoding.UTF8,
                            "application/json"
                        );
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize Keycloak error response.");
                response.Content = new StringContent(
                    JsonConvert.SerializeObject(new { error = "Invalid error response format." }),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            return response;
        }

        private record KeycloakErrorDetails
        {
            public string Error { get; set; }
            public string ErrorDescription { get; set; }
        }
    }
}