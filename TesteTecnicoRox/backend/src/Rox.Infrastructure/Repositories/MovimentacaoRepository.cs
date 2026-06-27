using Microsoft.EntityFrameworkCore;
using Rox.Application.Abstractions;
using Rox.Domain.Entities;
using Rox.Infrastructure.Persistence;

namespace Rox.Infrastructure.Repositories;

public sealed class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly RoxDbContext _context;

    public MovimentacaoRepository(RoxDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MovimentacaoEstoque>> ListarAsync(CancellationToken cancellationToken) =>
        await _context.Movimentacoes
            .AsNoTracking()
            .Include(m => m.Produto)
            .ToListAsync(cancellationToken);

    public Task<MovimentacaoEstoque?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Movimentacoes.Include(m => m.Produto).FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public Task<bool> ExisteParaProdutoAsync(Guid produtoId, CancellationToken cancellationToken) =>
        _context.Movimentacoes.AnyAsync(m => m.ProdutoId == produtoId, cancellationToken);

    public void Adicionar(MovimentacaoEstoque movimentacao) => _context.Movimentacoes.Add(movimentacao);

    public void Remover(MovimentacaoEstoque movimentacao) => _context.Movimentacoes.Remove(movimentacao);
}
