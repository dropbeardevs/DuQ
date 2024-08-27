using Microsoft.AspNetCore.Identity;
using DuQ.Contexts;

namespace DuQ.Components.Account;

internal sealed class IdentityUserAccessor(UserManager<DuQIdentityUser> userManager, IdentityRedirectManager redirectManager)
{
    public async Task<DuQIdentityUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}
