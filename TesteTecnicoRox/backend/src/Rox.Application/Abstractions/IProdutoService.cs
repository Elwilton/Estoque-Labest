using Rox.Application.Common;
using Rox.Application.DTOs.Produtos;

namespace Rox.Application.Abstractions;

public interface IProdutoService
{
    Task<IReadOnlyList<ProdutoResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<Result<ProdutoResponse>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<ProdutoResponse>> CriarAsync(ProdutoRequest request, CancellationToken cancellationToken);
    Task<Result<ProdutoResponse>> AtualizarAsync(Guid id, ProdutoUpdateRequest request, CancellationToken cancellationToken);
    Task<Result> RemoverAsync(Guid id, CancellationToken cancellationToken);
}
