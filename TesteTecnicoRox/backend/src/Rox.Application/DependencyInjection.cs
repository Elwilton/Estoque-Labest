using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Rox.Application.Abstractions;
using Rox.Application.Services;
using Rox.Application.Validators;

namespace Rox.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IMovimentacaoService, MovimentacaoService>();

        return services;
    }
}
