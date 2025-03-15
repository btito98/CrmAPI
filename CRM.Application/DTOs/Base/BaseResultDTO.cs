using System.Text.Json.Serialization;

namespace CRM.Application.DTOs
{
    public record BaseResultDTO
    {
        public Guid Id { get; init; }
        public string UsuarioCriacao { get; init; }
        public DateTime DataCriacao { get; init; }
        [JsonIgnore]
        public string? UsuarioAlteracao { get; init; }
        [JsonIgnore]
        public DateTime? DataAlteracao { get; init; }
        public bool Ativo { get; init; }
    }
}
