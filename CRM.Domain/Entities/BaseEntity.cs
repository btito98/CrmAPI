namespace CRM.Domain.Entities
{
    public abstract class BaseEntity
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public string UsuarioCriacao { get; private set; }
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public string? UsuarioAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; private set; } = true;

        public virtual void Activate() => Ativo = true; 
        public virtual void Deactivate() => Ativo = false;
        public virtual void Update(string usuarioAlteracao)
        {
            UsuarioAlteracao = usuarioAlteracao;
            DataAlteracao = DateTime.UtcNow;
        }
    }
}
