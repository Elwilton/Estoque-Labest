using Rox.Domain.Entities;

namespace Rox.Application.Abstractions;

public interface IMovimentacaoRepository
{
    Task<IReadOnlyList<MovimentacaoEstoque>> ListarAsync(CancellationToken cancellationToken);
    Task<MovimentacaoEstoque?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExisteParaProdutoAsync(Guid produtoId, CancellationToken cancellationToken);
    void Adicionar(MovimentacaoEstoque movimentacao);
    void Remover(MovimentacaoEstoque movimentacao);
}
