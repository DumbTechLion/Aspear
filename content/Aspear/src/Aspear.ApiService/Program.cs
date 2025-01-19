using Aspear.AppHost;
using Aspear.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults(ServiceNames.ApiService);
builder.Services
    .AddAuthentication(options =>  
    {  
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
    })
    .AddKeycloakJwtBearer(ServiceNames.Keycloak, realm: "AspireKeycloak", options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

var res = Random.Shared.Next(1, 100);
var truc = res switch
{
    >=50 and <= 70 => 1,
    >= 10 => 2,
    _ => 0
};

Console.WriteLine(truc);

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
