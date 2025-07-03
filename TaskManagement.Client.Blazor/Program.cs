using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskManagement.Client.Blazor;
using TaskManagement.Client.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTelerikBlazor();
builder.Services.AddHttpClient("AuthorizedAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7171/");
});

builder.Services.AddScoped<TaskService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthorizedHttpClient>();
builder.Services.AddSingleton<SignalRService>();
builder.Services.AddScoped<AuthService>();



await builder.Build().RunAsync();
