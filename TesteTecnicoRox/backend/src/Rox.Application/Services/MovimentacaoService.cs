using Rox.Application.Abstractions;
using Rox.Application.Common;
using Rox.Application.DTOs.Movimentacoes;
using Rox.Domain.Entities;
using Rox.Domain.Exceptions;

namespace Rox.Application.Services;

public sealed class MovimentacaoService : IMovimentacaoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public MovimentacaoService(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<MovimentacaoResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        var movimentacoes = await _unitOfWork.Movimentacoes.ListarAsync(cancellationToken);

        return movimentacoes
            .OrderByDescending(m => m.CriadoEm)
            .Select(m => new MovimentacaoResponse(
                m.Id,
                m.ProdutoId,
                m.Produto?.Nome ?? string.Empty,
                m.Tipo,
                m.Quantidade,
                m.Observacao,
                m.CriadoEm))
            .ToList();
    }

    public async Task<Result<MovimentacaoResponse>> RegistrarAsync(MovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(request.ProdutoId, cancellationToken);

        if (produto is null)
            return Result<MovimentacaoResponse>.Falha("Produto não encontrado.");

        try
        {
            var movimentacao = MovimentacaoEstoque.Criar(produto, request.Tipo, request.Quantidade, _currentUser.UserId, request.Observacao);

            _unitOfWork.Movimentacoes.Adicionar(movimentacao);
            await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

            return Result<MovimentacaoResponse>.Ok(new MovimentacaoResponse(
                movimentacao.Id,
                produto.Id,
                produto.Nome,
                movimentacao.Tipo,
                movimentacao.Quantidade,
                movimentacao.Observacao,
                movimentacao.CriadoEm));
        }
        catch (DomainException ex)
        {
            return Result<MovimentacaoResponse>.Falha(ex.Message);
        }
    }

    public async Task<Result<MovimentacaoResponse>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var movimentacao = await _unitOfWork.Movimentacoes.ObterPorIdAsync(id, cancellationToken);
        if (movimentacao is null)
            return Result<MovimentacaoResponse>.Falha("Movimentação não encontrada.");

        return Result<MovimentacaoResponse>.Ok(new MovimentacaoResponse(
            movimentacao.Id,
            movimentacao.ProdutoId,
            movimentacao.Produto?.Nome ?? string.Empty,
            movimentacao.Tipo,
            movimentacao.Quantidade,
            movimentacao.Observacao,
            movimentacao.CriadoEm));
    }

    public async Task<Result<MovimentacaoResponse>> AtualizarAsync(Guid id, MovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var movimentacao = await _unitOfWork.Movimentacoes.ObterPorIdAsync(id, cancellationToken);
        if (movimentacao is null)
            return Result<MovimentacaoResponse>.Falha("Movimentação não encontrada.");

        var produtoOriginal = await _unitOfWork.Produtos.ObterPorIdAsync(movimentacao.ProdutoId, cancellationToken);
        if (produtoOriginal is null)
            return Result<MovimentacaoResponse>.Falha("Produto associado à movimentação não foi encontrado.");

        try
        {
            Produto produtoDestino;

            if (request.ProdutoId == produtoOriginal.Id)
            {
                produtoOriginal.AjustarPorEdicaoDeMovimentacao(
                    movimentacao.Tipo, movimentacao.Quantidade,
                    request.Tipo, request.Quantidade);

                produtoDestino = produtoOriginal;
            }
            else
            {
                var produtoNovo = await _unitOfWork.Produtos.ObterPorIdAsync(request.ProdutoId, cancellationToken);
                if (produtoNovo is null)
                    return Result<MovimentacaoResponse>.Falha("Produto não encontrado.");

                produtoOriginal.DesfazerMovimentacao(movimentacao.Tipo, movimentacao.Quantidade);

                if (request.Tipo == Domain.Enums.TipoMovimentacao.Entrada)
                    produtoNovo.RegistrarEntrada(request.Quantidade);
                else
                    produtoNovo.RegistrarSaida(request.Quantidade);

                produtoDestino = produtoNovo;
            }

            movimentacao.AtualizarDados(produtoDestino.Id, request.Tipo, request.Quantidade, request.Observacao);
            await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

            return Result<MovimentacaoResponse>.Ok(new MovimentacaoResponse(
                movimentacao.Id,
                produtoDestino.Id,
                produtoDestino.Nome,
                movimentacao.Tipo,
                movimentacao.Quantidade,
                movimentacao.Observacao,
                movimentacao.CriadoEm));
        }
        catch (DomainException ex)
        {
            return Result<MovimentacaoResponse>.Falha(ex.Message);
        }
    }

    public async Task<Result> RemoverAsync(Guid id, CancellationToken cancellationToken)
    {
        var movimentacao = await _unitOfWork.Movimentacoes.ObterPorIdAsync(id, cancellationToken);
        if (movimentacao is null)
            return Result.Falha("Movimentação não encontrada.");

        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(movimentacao.ProdutoId, cancellationToken);
        if (produto is null)
            return Result.Falha("Produto associado à movimentação não foi encontrado.");

        try
        {
            produto.DesfazerMovimentacao(movimentacao.Tipo, movimentacao.Quantidade);
            _unitOfWork.Movimentacoes.Remover(movimentacao);
            await _unitOfWork.SalvarAlteracoesAsync(cancellationToken);

            return Result.Ok();
        }
        catch (DomainException ex)
        {
            return Result.Falha(ex.Message);
        }
    }
}
