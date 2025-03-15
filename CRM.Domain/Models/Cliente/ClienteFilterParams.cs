namespace CRM.Domain.Models.Cliente
{
    public record ClienteFilterParams : BaseFilterParams
    {
        public string? Nome { get; init; }
        public string? Email { get; init; }
        public string? Telefone { get; init; }
    }
}
