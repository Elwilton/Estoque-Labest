using Microsoft.AspNetCore.Identity;

namespace Rox.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string Nome { get; set; } = string.Empty;
}
