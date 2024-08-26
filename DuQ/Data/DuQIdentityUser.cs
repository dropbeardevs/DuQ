using Microsoft.AspNetCore.Identity;

namespace DuQ.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class DuQIdentityUser : Microsoft.AspNetCore.Identity.IdentityUser
{
    public ICollection<IdentityUserRole<string>> Roles { get; set; }
    public ICollection<IdentityUserClaim<string>> Claims { get; set; }
}
