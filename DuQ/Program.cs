using DuQ.Components;
using DuQ.Core;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string dbFileLocation = builder.Configuration.GetValue<string>("DbFileLocation") ?? throw new NullReferenceException("Missing DbFileLocation");



builder.Services.AddDbContextFactory<DuqContext>(
    options => options.UseSqlite($"Data Source={dbFileLocation}"));

builder.Services.AddTransient<DuQ.Components.Pages.Checkin.Domain>();
builder.Services.AddTransient<DuQ.Components.Pages.Status.Domain>();
builder.Services.AddTransient<DuQ.Components.Pages.Admin.Domain>();
builder.Services.AddSingleton<DbSaveNotifier>();

//builder.Services.AddHttpClient();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
