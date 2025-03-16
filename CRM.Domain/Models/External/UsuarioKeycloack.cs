using Newtonsoft.Json;

namespace CRM.Domain.Models.External
{
    public record UsuarioKeycloack
    {
        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string Nome { get; set; }

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string Sobrenome { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}
