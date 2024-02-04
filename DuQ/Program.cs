using DuQ.Components;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string dbFileLocation = builder.Configuration.GetValue<string>("DbFileLocation") ?? throw new NullReferenceException("Missing DbFileLocation");



builder.Services.AddDbContext<DuqContext>(
    options => options.UseSqlite($"Data Source={dbFileLocation}"));

builder.Services.AddTransient<DuQ.Components.Pages.Checkin.Domain>();
builder.Services.AddTransient<DuQ.Components.Pages.Status.Domain>();


builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();

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
