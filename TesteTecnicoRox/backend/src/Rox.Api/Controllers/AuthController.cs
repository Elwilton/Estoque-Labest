using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Rox.Api.Extensions;
using Rox.Application.Abstractions;
using Rox.Application.DTOs.Auth;

namespace Rox.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _registerValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToBadRequest();

        var resultado = await _authService.RegistrarAsync(request, cancellationToken);
        if (!resultado.Sucesso)
            return BadRequest(new { erro = resultado.Erro });

        return Ok(resultado.Valor);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _loginValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToBadRequest();

        var resultado = await _authService.LoginAsync(request, cancellationToken);
        if (!resultado.Sucesso)
            return Unauthorized(new { erro = resultado.Erro });

        return Ok(resultado.Valor);
    }
}
