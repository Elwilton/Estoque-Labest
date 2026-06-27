using Rox.Domain.Enums;
using Rox.Domain.Exceptions;

namespace Rox.Domain.Entities;

public sealed class MovimentacaoEstoque : Entity
{
    public Guid ProdutoId { get; private set; }
    public Produto? Produto { get; private set; }
    public TipoMovimentacao Tipo { get; private set; }
    public int Quantidade { get; private set; }
    public string? Observacao { get; private set; }
    public string UsuarioId { get; private set; } = string.Empty;

    private MovimentacaoEstoque()
    {
    }

    public static MovimentacaoEstoque Criar(Produto produto, TipoMovimentacao tipo, int quantidade, string usuarioId, string? observacao)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade da movimentação deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(usuarioId))
            throw new DomainException("A movimentação deve estar associada a um usuário.");

        if (tipo == TipoMovimentacao.Entrada)
            produto.RegistrarEntrada(quantidade);
        else
            produto.RegistrarSaida(quantidade);

        return new MovimentacaoEstoque
        {
            ProdutoId = produto.Id,
            Tipo = tipo,
            Quantidade = quantidade,
            Observacao = observacao?.Trim(),
            UsuarioId = usuarioId
        };
    }

    public void AtualizarDados(Guid produtoId, TipoMovimentacao tipo, int quantidade, string? observacao)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade da movimentação deve ser maior que zero.");

        ProdutoId = produtoId;
        Tipo = tipo;
        Quantidade = quantidade;
        Observacao = observacao?.Trim();
    }
}
