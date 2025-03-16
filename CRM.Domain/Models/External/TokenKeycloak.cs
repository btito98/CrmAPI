using Newtonsoft.Json;

namespace CRM.Domain.Models.External
{
    public record TokenKeycloak
    {
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public int RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }

        [JsonProperty("not-before-policy", NullValueHandling = NullValueHandling.Ignore)]
        public int NotBeforePolicy { get; set; }

        [JsonProperty("session_state", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionState { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; set; }

    }
}
