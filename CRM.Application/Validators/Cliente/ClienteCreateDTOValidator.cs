using CRM.Application.DTOs.Cliente;
using FluentValidation;

namespace CRM.Application.Validators.Cliente
{
    public class ClienteCreateDTOValidator : AbstractValidator<ClienteCreateDTO>
    {
        public ClienteCreateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres");
        }
    }
}
