namespace CRM.Application.DTOs.Cliente
{
    public record ClienteCreateDTO : BaseCreateDTO
    {
        public string Nome { get; init; }
        public string Email { get; init; }
        public string Telefone { get; init; }
    }
}
