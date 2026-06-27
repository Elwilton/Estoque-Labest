using FluentValidation;
using Rox.Application.DTOs.Produtos;

namespace Rox.Application.Validators;

public sealed class ProdutoUpdateRequestValidator : AbstractValidator<ProdutoUpdateRequest>
{
    public ProdutoUpdateRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.Descricao)
            .MaximumLength(1000);

        RuleFor(x => x.Preco)
            .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.");
    }
}
