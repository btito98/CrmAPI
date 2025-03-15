using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRM.Application.DTOs
{
    public record BaseCreateDTO
    {
        [JsonIgnore]
        public string? UsuarioCriacao { get; private set; }
        [JsonIgnore]
        public string? UsuarioAlteracao { get; private set; }
        [JsonIgnore]
        public bool Ativo { get; init; } = true;

        public void InitializeUserCreation(string usuarioCriacao)
        {
            if (string.IsNullOrWhiteSpace(usuarioCriacao))
                throw new ArgumentNullException(nameof(usuarioCriacao), "O usuário de criação é obrigatório.");

            UsuarioCriacao = usuarioCriacao;
        }

        public void UpdateUser(string usuarioAlteracao)
        {
            if (string.IsNullOrWhiteSpace(usuarioAlteracao))
                throw new ArgumentNullException(nameof(usuarioAlteracao), "O usuário de alteração é obrigatório.");

            UsuarioAlteracao = usuarioAlteracao;
        }
    }
}
