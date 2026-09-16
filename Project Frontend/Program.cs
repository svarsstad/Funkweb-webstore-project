using Project_Frontend.Components;
using Project_Frontend.Services;
using Project_Backend.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(
    "appsettings.private.json",
    optional: false,
    reloadOnChange: true);

// Minimal template setup for Razor Components interactive server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Optional: register HttpClient to call backend API if needed
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7135/")
});
builder.Services.AddScoped<PublicProductService>();
// Register CurrencyService so components that inject Project_Backend.Services.CurrencyService can resolve it
builder.Services.AddSingleton<CurrencyService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
