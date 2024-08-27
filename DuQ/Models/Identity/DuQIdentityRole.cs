using Microsoft.AspNetCore.Identity;

namespace DuQ.Models.Identity;

public class DuQIdentityRole : IdentityRole
{
    public DuQIdentityRole() { }

    public DuQIdentityRole(string roleName)
        : base(roleName) { }

    public ICollection<IdentityRoleClaim<string>> Claims { get; set; }
}
