using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rox.Api.Extensions;
using Rox.Application.Abstractions;
using Rox.Application.DTOs.Movimentacoes;

namespace Rox.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/movimentacoes")]
public sealed class MovimentacoesController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;
    private readonly IValidator<MovimentacaoRequest> _validator;

    public MovimentacoesController(IMovimentacaoService movimentacaoService, IValidator<MovimentacaoRequest> validator)
    {
        _movimentacaoService = movimentacaoService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var movimentacoes = await _movimentacaoService.ListarAsync(cancellationToken);
        return Ok(movimentacoes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _movimentacaoService.ObterPorIdAsync(id, cancellationToken);
        if (!resultado.Sucesso)
            return NotFound(new { erro = resultado.Erro });

        return Ok(resultado.Valor);
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] MovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToBadRequest();

        var resultado = await _movimentacaoService.RegistrarAsync(request, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return CreatedAtAction(nameof(Listar), resultado.Valor);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] MovimentacaoRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToBadRequest();

        var resultado = await _movimentacaoService.AtualizarAsync(id, request, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return Ok(resultado.Valor);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _movimentacaoService.RemoverAsync(id, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return NoContent();
    }
}
