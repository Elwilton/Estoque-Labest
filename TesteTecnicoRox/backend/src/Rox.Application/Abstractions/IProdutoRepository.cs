using Rox.Domain.Entities;

namespace Rox.Application.Abstractions;

public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Produto>> ListarAsync(CancellationToken cancellationToken);
    void Adicionar(Produto produto);
    void Remover(Produto produto);
}
