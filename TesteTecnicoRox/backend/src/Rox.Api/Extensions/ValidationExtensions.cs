using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Rox.Api.Extensions;

public static class ValidationExtensions
{
    public static IActionResult ToBadRequest(this ValidationResult validationResult)
    {
        var erros = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new ValidationProblemDetails(erros));
    }
}
