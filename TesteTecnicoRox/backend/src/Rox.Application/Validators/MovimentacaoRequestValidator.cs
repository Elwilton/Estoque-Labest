using FluentValidation;
using Rox.Application.DTOs.Movimentacoes;

namespace Rox.Application.Validators;

public sealed class MovimentacaoRequestValidator : AbstractValidator<MovimentacaoRequest>
{
    public MovimentacaoRequestValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty().WithMessage("O produto é obrigatório.");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("O tipo de movimentação informado é inválido.");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.Observacao)
            .MaximumLength(500);
    }
}
