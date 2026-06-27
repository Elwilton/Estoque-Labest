using Rox.Application.Abstractions;
using Rox.Application.Common;
using Rox.Application.DTOs.Produtos;
using Rox.Domain.Entities;
using Rox.Domain.Exceptions;

namespace Rox.Application.Services;

public sealed class ProdutoService : IProdutoService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ProdutoResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        var produtos = await _unitOfWork.Produtos.ListarAsync(cancellationToken);

        return produtos
            .Select(ToResponse)
            .OrderBy(p => p.Nome)
            .ToList();
    }

    public async Task<Result<ProdutoResponse>> CriarAsync(ProdutoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var produto = Produto.Criar(request.Nome, request.Descricao, request.Preco, request.QuantidadeInicial);

            _unitOfWork.Produtos.Adicionar(produto);
            await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

            return Result<ProdutoResponse>.Ok(ToResponse(produto));
        }
        catch (DomainException ex)
        {
            return Result<ProdutoResponse>.Falha(ex.Message);
        }
    }

    public async Task<Result<ProdutoResponse>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id, cancellationToken);
        if (produto is null)
            return Result<ProdutoResponse>.Falha("Produto não encontrado.");

        return Result<ProdutoResponse>.Ok(ToResponse(produto));
    }

    public async Task<Result<ProdutoResponse>> AtualizarAsync(Guid id, ProdutoUpdateRequest request, CancellationToken cancellationToken)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id, cancellationToken);
        if (produto is null)
            return Result<ProdutoResponse>.Falha("Produto não encontrado.");

        try
        {
            produto.Atualizar(request.Nome, request.Descricao, request.Preco);
            await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

            return Result<ProdutoResponse>.Ok(ToResponse(produto));
        }
        catch (DomainException ex)
        {
            return Result<ProdutoResponse>.Falha(ex.Message);
        }
    }

    public async Task<Result> RemoverAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id, cancellationToken);
        if (produto is null)
            return Result.Falha("Produto não encontrado.");

        var possuiMovimentacoes = await _unitOfWork.Movimentacoes.ExisteParaProdutoAsync(id, cancellationToken);
        if (possuiMovimentacoes)
            return Result.Falha("Não é possível remover um produto que possui movimentações de estoque registradas.");

        _unitOfWork.Produtos.Remover(produto);
        await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return Result.Ok();
    }

    private static ProdutoResponse ToResponse(Produto produto) =>
        new(produto.Id, produto.Nome, produto.Descricao, produto.Preco, produto.QuantidadeEmEstoque, produto.CriadoEm);
}
