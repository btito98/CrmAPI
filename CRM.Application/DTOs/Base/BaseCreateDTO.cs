using System.Text.Json.Serialization;

namespace CRM.Application.DTOs
{
    public record BaseCreateDTO
    {
        public string UsuarioCriacao { get; init; }
        [JsonIgnore]
        public bool Ativo { get; init; } = true;
    }
}
