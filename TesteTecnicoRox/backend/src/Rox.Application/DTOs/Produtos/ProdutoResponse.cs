namespace Rox.Application.DTOs.Produtos;

public sealed record ProdutoResponse(Guid Id, string Nome, string? Descricao, decimal Preco, int QuantidadeEmEstoque, DateTime CriadoEm);
