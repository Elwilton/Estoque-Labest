using Microsoft.AspNetCore.Identity;
using Rox.Application.Abstractions;
using Rox.Application.Common;
using Rox.Application.DTOs.Auth;

namespace Rox.Infrastructure.Identity;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<AuthResponse>> RegistrarAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var usuarioExistente = await _userManager.FindByEmailAsync(request.Email);
        if (usuarioExistente is not null)
            return Result<AuthResponse>.Falha("Já existe um usuário cadastrado com este e-mail.");

        var usuario = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            Nome = request.Nome
        };

        var resultado = await _userManager.CreateAsync(usuario, request.Senha);
        if (!resultado.Succeeded)
        {
            var erros = string.Join(" ", resultado.Errors.Select(e => e.Description));
            return Result<AuthResponse>.Falha(erros);
        }

        var (token, expiraEm) = _tokenGenerator.Gerar(usuario);
        return Result<AuthResponse>.Ok(new AuthResponse(token, expiraEm, usuario.Nome, usuario.Email!));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email);
        if (usuario is null)
            return Result<AuthResponse>.Falha("E-mail ou senha inválidos.");

        var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Senha, lockoutOnFailure: true);
        if (!resultado.Succeeded)
            return Result<AuthResponse>.Falha("E-mail ou senha inválidos.");

        var (token, expiraEm) = _tokenGenerator.Gerar(usuario);
        return Result<AuthResponse>.Ok(new AuthResponse(token, expiraEm, usuario.Nome, usuario.Email!));
    }
}
