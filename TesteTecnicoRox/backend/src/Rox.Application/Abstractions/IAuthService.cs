using Rox.Application.Common;
using Rox.Application.DTOs.Auth;

namespace Rox.Application.Abstractions;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegistrarAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
