using Blazored.LocalStorage;
using Homie.Components;
using Homie.Interfaces;
using Homie.Db;
using Homie.Services;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor
builder.Services.AddMudServices();
MudGlobal.InputDefaults.Variant = Variant.Outlined;
MudGlobal.InputDefaults.Margin = Margin.Dense;
MudGlobal.Rounded = true;
MudGlobal.GridDefaults.Spacing = 3;

// Add Local storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddDataProtection();

// Add BrowserDateTimeService
builder.Services.AddScoped<IBrowserDateTimeService, BrowserDateTimeService>();

// Add Db
builder.Services.AddScoped<IHomieDbService, HomieDbService>(sp =>
{
    var connectionString = Environment.GetEnvironmentVariable("ConnectionString__Homie");
    return new HomieDbService(connectionString);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();