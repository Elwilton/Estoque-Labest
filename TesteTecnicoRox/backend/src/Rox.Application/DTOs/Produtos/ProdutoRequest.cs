namespace Rox.Application.DTOs.Produtos;

public sealed record ProdutoRequest(string Nome, string? Descricao, decimal Preco, int QuantidadeInicial);
