namespace Rox.Application.Abstractions;

public interface IUnitOfWork
{
    IProdutoRepository Produtos { get; }
    IMovimentacaoRepository Movimentacoes { get; }
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
}
