using Rox.Domain.Enums;

namespace Rox.Application.DTOs.Movimentacoes;

public sealed record MovimentacaoResponse(
    Guid Id,
    Guid ProdutoId,
    string ProdutoNome,
    TipoMovimentacao Tipo,
    int Quantidade,
    string? Observacao,
    DateTime CriadoEm);
