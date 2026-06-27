using Rox.Application.Abstractions;
using Rox.Infrastructure.Persistence;

namespace Rox.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly RoxDbContext _context;

    public UnitOfWork(RoxDbContext context, IProdutoRepository produtos, IMovimentacaoRepository movimentacoes)
    {
        _context = context;
        Produtos = produtos;
        Movimentacoes = movimentacoes;
    }

    public IProdutoRepository Produtos { get; }
    public IMovimentacaoRepository Movimentacoes { get; }

    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
