using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rox.Domain.Entities;
using Rox.Infrastructure.Identity;

namespace Rox.Infrastructure.Persistence;

public sealed class RoxDbContext : IdentityDbContext<ApplicationUser>
{
    public RoxDbContext(DbContextOptions<RoxDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<MovimentacaoEstoque> Movimentacoes => Set<MovimentacaoEstoque>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Produto>(entity =>
        {
            entity.ToTable("Produtos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Descricao).HasMaxLength(1000);
            entity.Property(p => p.Preco).HasColumnType("decimal(18,2)");
        });

        builder.Entity<MovimentacaoEstoque>(entity =>
        {
            entity.ToTable("Movimentacoes");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Observacao).HasMaxLength(500);
            entity.Property(m => m.UsuarioId).IsRequired();

            entity.HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
