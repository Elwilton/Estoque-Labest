using Rox.Domain.Enums;
using Rox.Domain.Exceptions;

namespace Rox.Domain.Entities;

public sealed class Produto : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public int QuantidadeEmEstoque { get; private set; }

    private Produto()
    {
    }

    public static Produto Criar(string nome, string? descricao, decimal preco, int quantidadeInicial)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do produto é obrigatório.");

        if (preco < 0)
            throw new DomainException("O preço do produto não pode ser negativo.");

        if (quantidadeInicial < 0)
            throw new DomainException("A quantidade inicial não pode ser negativa.");

        return new Produto
        {
            Nome = nome.Trim(),
            Descricao = descricao?.Trim(),
            Preco = preco,
            QuantidadeEmEstoque = quantidadeInicial
        };
    }

    public void Atualizar(string nome, string? descricao, decimal preco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do produto é obrigatório.");

        if (preco < 0)
            throw new DomainException("O preço do produto não pode ser negativo.");

        Nome = nome.Trim();
        Descricao = descricao?.Trim();
        Preco = preco;
    }

    public void RegistrarEntrada(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade da movimentação deve ser maior que zero.");

        QuantidadeEmEstoque += quantidade;
    }

    public void RegistrarSaida(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade da movimentação deve ser maior que zero.");

        if (quantidade > QuantidadeEmEstoque)
            throw new DomainException("Estoque insuficiente para realizar a saída.");

        QuantidadeEmEstoque -= quantidade;
    }

    public void DesfazerMovimentacao(TipoMovimentacao tipo, int quantidade)
    {
        if (tipo == TipoMovimentacao.Entrada)
        {
            if (quantidade > QuantidadeEmEstoque)
                throw new DomainException("Não é possível remover esta movimentação: o estoque atual é menor que a quantidade que seria revertida.");

            QuantidadeEmEstoque -= quantidade;
        }
        else
        {
            QuantidadeEmEstoque += quantidade;
        }
    }

    public void AjustarPorEdicaoDeMovimentacao(
        TipoMovimentacao tipoAntigo,
        int quantidadeAntiga,
        TipoMovimentacao tipoNovo,
        int quantidadeNova)
    {
        if (quantidadeNova <= 0)
            throw new DomainException("A quantidade da movimentação deve ser maior que zero.");

        var efeitoAntigo = tipoAntigo == TipoMovimentacao.Entrada ? quantidadeAntiga : -quantidadeAntiga;
        var efeitoNovo = tipoNovo == TipoMovimentacao.Entrada ? quantidadeNova : -quantidadeNova;
        var novoEstoque = QuantidadeEmEstoque - efeitoAntigo + efeitoNovo;

        if (novoEstoque < 0)
            throw new DomainException("Estoque insuficiente para realizar a saída.");

        QuantidadeEmEstoque = novoEstoque;
    }
}
