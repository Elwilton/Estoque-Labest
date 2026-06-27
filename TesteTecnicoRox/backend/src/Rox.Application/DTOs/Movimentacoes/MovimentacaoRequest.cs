using Rox.Domain.Enums;

namespace Rox.Application.DTOs.Movimentacoes;

public sealed record MovimentacaoRequest(Guid ProdutoId, TipoMovimentacao Tipo, int Quantidade, string? Observacao);
