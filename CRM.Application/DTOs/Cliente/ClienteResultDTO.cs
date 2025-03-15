namespace CRM.Application.DTOs.Cliente
{
    public record ClienteResultDTO : BaseResultDTO
    {
        public string Nome { get; init; }
        public string Email { get; init; }
        public string Telefone { get; init; }
    }
}
