using Microsoft.AspNetCore.Identity;

namespace TestersManager.Core.Domain.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? TesterName { get; set; }
}