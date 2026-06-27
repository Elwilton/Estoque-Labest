using Microsoft.EntityFrameworkCore;
using Rox.Application.Abstractions;
using Rox.Domain.Entities;
using Rox.Infrastructure.Persistence;

namespace Rox.Infrastructure.Repositories;

public sealed class ProdutoRepository : IProdutoRepository
{
    private readonly RoxDbContext _context;

    public ProdutoRepository(RoxDbContext context)
    {
        _context = context;
    }

    public Task<Produto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Produtos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Produto>> ListarAsync(CancellationToken cancellationToken) =>
        await _context.Produtos.AsNoTracking().ToListAsync(cancellationToken);

    public void Adicionar(Produto produto) => _context.Produtos.Add(produto);

    public void Remover(Produto produto) => _context.Produtos.Remove(produto);
}
