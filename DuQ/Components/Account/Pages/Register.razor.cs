using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using DuQ.Components.Account.Pages.Manage;
using DuQ.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace DuQ.Components.Account.Pages;

public partial class Register
{
    private IEnumerable<IdentityError>? identityErrors;

    [Inject] private IConfiguration Configuration { get; set; } = null!;

    [SupplyParameterFromForm] private InputModel Input { get; set; } = new();

    [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

    private string? Message => identityErrors is null
        ? null
        : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

    private ValidationMessageStore? messageStore;

    private List<string> AllowedRegisterDomains = [];

    protected override void OnInitialized()
    {
        AllowedRegisterDomains = Configuration.GetSection("AllowedRegisterDomains").Get<List<string>>() ?? [];
    }

    public async Task RegisterUser(EditContext editContext)
    {
        editContext.OnValidationRequested += HandleValidationRequested;
        messageStore = new(editContext);

        if (!editContext.Validate())
        {
            return;
        }

        var user = CreateUser();

        await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        var result = await UserManager.CreateAsync(user, Input.Password);

        if (!result.Succeeded)
        {
            identityErrors = result.Errors;
            return;
        }

        Logger.LogInformation("User created a new account with password.");

        var userId = await UserManager.GetUserIdAsync(user);
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
            new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });

        await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

        if (UserManager.Options.SignIn.RequireConfirmedAccount)
        {
            RedirectManager.RedirectTo(
                "Account/RegisterConfirmation",
                new() { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });
        }

        await SignInManager.SignInAsync(user, isPersistent: false);
        RedirectManager.RedirectTo(ReturnUrl);
    }

    private DuQIdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<DuQIdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(DuQIdentityUser)}'. " +
                                                $"Ensure that '{nameof(DuQIdentityUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private IUserEmailStore<DuQIdentityUser> GetEmailStore()
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<DuQIdentityUser>)UserStore;
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
    {
        messageStore?.Clear();

        // Custom validation logic

        Uri uri = new($"mailto:{Input.Email}");

        if (AllowedRegisterDomains.Any(a => a.Contains(uri.Host)) is false)
        {
            messageStore?.Add(() => Input.Email, "Invalid email domain");
        }
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
