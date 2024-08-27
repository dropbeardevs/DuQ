using DuQ.Contexts;
using DuQ.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using Serilog;

namespace DuQ.Components.Account;

public class EmailSender: IEmailSender<ApplicationUser>
{
    private readonly IConfiguration _configuration;
    private string _postmarkToken;
    private string _emailFromAddress;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;

        _postmarkToken = _configuration.GetValue<string>("PostmarkToken") ??
                         throw new ArgumentNullException(_postmarkToken);

        _emailFromAddress = _configuration.GetValue<string>("EmailFromAddress") ??
                            throw new ArgumentNullException(_emailFromAddress);
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email,
        string confirmationLink) => SendEmailAsync(email, "Confirm your email",
             "Please confirm your account by " +
             $"<a href='{confirmationLink}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email,
        string resetLink) => SendEmailAsync(email, "Reset your password",
        $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email,
        string resetCode) => SendEmailAsync(email, "Reset your password",
        $"Please reset your password using the following code: {resetCode}");

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        Log.Debug($"To: {toEmail}, Subject: {subject}, Message: {message}");

        var emailMessage = new PostmarkMessage()
                      {
                          To = toEmail,
                          From = _emailFromAddress,
                          TrackOpens = true,
                          Subject = subject,
                          HtmlBody = message
                      };

        var client = new PostmarkClient(_postmarkToken);

        var sendResult = await client.SendMessageAsync(emailMessage);

        if (sendResult.Status == PostmarkStatus.Success)
        {
            Log.Information("Email sent successfully");
        }
        else
        {
            Log.Error("Email send failed");
        }
    }
}
