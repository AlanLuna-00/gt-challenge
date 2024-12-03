using System.Net.Http.Json;
using Blazored.LocalStorage;
using client;
using client.Services;
using client.Shared.Handlers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Registrar los componentes principales
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string apiBaseUrl;

if (builder.HostEnvironment.IsDevelopment())
{
    apiBaseUrl = "http://localhost:5274/";
}
else
{
    apiBaseUrl = "http://localhost:3000/";
}

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});

// Configuración de servicios
builder.Services.AddBlazoredLocalStorage(); // Servicio para almacenamiento local
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore(); // Autorización en Blazor
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddHttpClient("AuthorizedClient")
    .AddHttpMessageHandler<JwtAuthorizationHandler>();
builder.Services.AddTransient<JwtAuthorizationHandler>();


builder.Services.AddHttpClient("GT-API", client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GT-API"));

await builder.Build().RunAsync();