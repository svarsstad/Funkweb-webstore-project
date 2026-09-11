using Project_Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// Minimal template setup for Razor Components interactive server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Optional: register HttpClient to call backend API if needed
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7135/")
});

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
