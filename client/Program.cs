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

// Leer la configuración desde appsettings.json en wwwroot
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var config = await httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, string>>>("appsettings.json");

if (config == null || !config.ContainsKey("ApiSettings") || !config["ApiSettings"].ContainsKey("BaseUrl"))
{
    throw new InvalidOperationException("API Base URL is missing in appsettings.json");
}

var apiBaseUrl = config["ApiSettings"]["BaseUrl"];

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





// Registrar el cliente HTTP con la URL base
builder.Services.AddHttpClient("GT-API", client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GT-API"));

await builder.Build().RunAsync();