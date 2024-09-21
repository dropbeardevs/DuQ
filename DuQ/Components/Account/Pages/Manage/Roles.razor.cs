using DuQ.Services;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Account.Pages.Manage;

public partial class Roles
{
    [Inject] IdentityService IdentityService { get; set; }
}
