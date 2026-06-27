using Rox.Application.Common;
using Rox.Application.DTOs.Movimentacoes;

namespace Rox.Application.Abstractions;

public interface IMovimentacaoService
{
    Task<IReadOnlyList<MovimentacaoResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<Result<MovimentacaoResponse>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<MovimentacaoResponse>> RegistrarAsync(MovimentacaoRequest request, CancellationToken cancellationToken);
    Task<Result<MovimentacaoResponse>> AtualizarAsync(Guid id, MovimentacaoRequest request, CancellationToken cancellationToken);
    Task<Result> RemoverAsync(Guid id, CancellationToken cancellationToken);
}
