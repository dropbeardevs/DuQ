using DuQ.Components;
using DuQ.Contexts;
using DuQ.Core;
using Microsoft.EntityFrameworkCore;
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

    // Add services to the container.
    builder.Services.AddRazorComponents()
           .AddInteractiveServerComponents();

    //string dbFileLocation = builder.Configuration.GetValue<string>("DbFileLocation") ?? throw new NullReferenceException("Missing DbFileLocation");

    string postgresConnectionString = builder.Configuration.GetConnectionString("DuQueue") ?? throw new NullReferenceException("Missing postgresConnectionString");

    //builder.Services.AddDbContextFactory<DuqContext>(
    //    options => options.UseSqlite($"Data Source={dbFileLocation}"));

    builder.Services.AddDbContextFactory<DuqContext>(
        options => options.UseNpgsql(postgresConnectionString)
                          .UseSnakeCaseNamingConvention()
    );

    builder.Services.AddTransient<DuQ.Components.Pages.Checkin.Domain>();
    builder.Services.AddTransient<DuQ.Components.Pages.Status.Domain>();
    builder.Services.AddTransient<DuQ.Components.Pages.Admin.Domain>();
    builder.Services.AddSingleton<DbSaveNotifier>();

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

    app.MapRazorComponents<App>()
       .AddInteractiveServerRenderMode();

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
