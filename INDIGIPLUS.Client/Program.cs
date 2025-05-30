using Blazored.LocalStorage;
using INDIGIPLUS.Client.Components;
using INDIGIPLUS.Client.Securities;
using INDIGIPLUS.Client.Services;
using INDIGIPLUS.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7295") });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IAuthClientService, AuthClientService>();
builder.Services.AddScoped<ILessonClientService, LessonClientService>();
builder.Services.AddScoped<IQuizClientService, QuizClientService>();
builder.Services.AddScoped<ITestConnectionClientService, TestConnectionClientService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();