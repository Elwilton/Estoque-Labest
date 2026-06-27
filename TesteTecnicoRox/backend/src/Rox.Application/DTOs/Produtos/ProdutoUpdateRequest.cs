namespace Rox.Application.DTOs.Produtos;

public sealed record ProdutoUpdateRequest(string Nome, string? Descricao, decimal Preco);
