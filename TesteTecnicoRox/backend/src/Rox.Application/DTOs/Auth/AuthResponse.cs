namespace Rox.Application.DTOs.Auth;

public sealed record AuthResponse(string Token, DateTime ExpiraEm, string Nome, string Email);
