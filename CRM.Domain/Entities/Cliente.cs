namespace CRM.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
