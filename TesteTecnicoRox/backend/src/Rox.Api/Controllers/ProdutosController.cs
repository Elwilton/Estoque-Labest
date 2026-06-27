using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rox.Api.Extensions;
using Rox.Application.Abstractions;
using Rox.Application.DTOs.Produtos;

namespace Rox.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/produtos")]
public sealed class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IValidator<ProdutoRequest> _validator;
    private readonly IValidator<ProdutoUpdateRequest> _updateValidator;

    public ProdutosController(
        IProdutoService produtoService,
        IValidator<ProdutoRequest> validator,
        IValidator<ProdutoUpdateRequest> updateValidator)
    {
        _produtoService = produtoService;
        _validator = validator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var produtos = await _produtoService.ListarAsync(cancellationToken);
        return Ok(produtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _produtoService.ObterPorIdAsync(id, cancellationToken);
        if (!resultado.Sucesso)
            return NotFound(new { erro = resultado.Erro });

        return Ok(resultado.Valor);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ProdutoRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToBadRequest();

        var resultado = await _produtoService.CriarAsync(request, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return CreatedAtAction(nameof(Listar), resultado.Valor);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProdutoUpdateRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToBadRequest();

        var resultado = await _produtoService.AtualizarAsync(id, request, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return Ok(resultado.Valor);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _produtoService.RemoverAsync(id, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return NoContent();
    }
}
