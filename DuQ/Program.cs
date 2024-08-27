using DuQ.Components;
using DuQ.Components.Account;
using DuQ.Contexts;
using DuQ.Core;
using DuQ.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .WriteTo.Console()
             .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, lc) => lc
                                                  .ReadFrom.Configuration(builder.Configuration)
                                                  .ReadFrom.Services(services)
                                                  .Enrich.FromLogContext()
                                                  .WriteTo.Console());

    builder.Services.AddLogging(loggingBuilder =>
        loggingBuilder.AddSerilog(dispose: true));

    // Add services to the container.
    builder.Services.AddRazorComponents()
           .AddInteractiveServerComponents();

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddScoped<IdentityUserAccessor>();
    builder.Services.AddScoped<IdentityRedirectManager>();
    builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

    builder.Services.AddAuthentication(options =>
           {
               options.DefaultScheme = IdentityConstants.ApplicationScheme;
               options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
           })
           .AddIdentityCookies();

    //string dbFileLocation = builder.Configuration.GetValue<string>("DbFileLocation") ?? throw new NullReferenceException("Missing DbFileLocation");

    string postgresConnectionString = builder.Configuration.GetConnectionString("DuQueue") ?? throw new NullReferenceException("Missing postgresConnectionString");
    string postgresIdentityConnectionString = builder.Configuration.GetConnectionString("DuQIdentity") ?? throw new NullReferenceException("Missing postgresIdentityConnectionString");

    //builder.Services.AddDbContextFactory<DuqContext>(
    //    options => options.UseSqlite($"Data Source={dbFileLocation}"));

    builder.Services.AddDbContextFactory<DuqContext>(
        options => options.UseNpgsql(postgresConnectionString)
                          .UseSnakeCaseNamingConvention()
    );

    builder.Services.AddDbContext<DuQIdentityDbContext>(options =>
        options.UseNpgsql(postgresIdentityConnectionString));
    //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentityCore<DuQIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
           //.AddRoles<DuQIdentityRole>()
           .AddEntityFrameworkStores<DuQIdentityDbContext>()
           .AddSignInManager()
           .AddDefaultTokenProviders();

    builder.Services.AddSingleton<IEmailSender<DuQIdentityUser>, EmailSender>();

    builder.Services.ConfigureApplicationCookie(options => {
        options.ExpireTimeSpan = TimeSpan.FromDays(5);
        options.SlidingExpiration = true;
    });

    builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
        options.TokenLifespan = TimeSpan.FromHours(3));

    // Set up CORS
    string[] allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? throw new NullReferenceException("Missing Allowed Origins");

    // Add services to the container.
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DuQueueCors", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .SetIsOriginAllowedToAllowWildcardSubdomains()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Services.AddTransient<DuQ.Components.Pages.Checkin.Domain>();
    builder.Services.AddTransient<DuQ.Components.Pages.Status.Domain>();
    builder.Services.AddTransient<DuQ.Components.Pages.Admin.Domain>();
    builder.Services.AddSingleton<DbSaveNotifier>();
    builder.Services.AddTransient<IdentityService>();

    //builder.Services.AddHttpClient();
    builder.Services.AddMudServices();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    //app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.UseCors("DuQueueCors");

    app.MapRazorComponents<App>()
       .AddInteractiveServerRenderMode();

    // app.UseAuthentication();
    // app.UseAuthorization();

    // Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
