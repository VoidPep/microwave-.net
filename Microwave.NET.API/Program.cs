using Microwave.NET.API.Config;
using Microwave.NET.API.Middleware;
using Microwave.NET.DataStructures.SignalR;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

DependencyInjection.InjectAll(builder);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseCors();

app.MapScalarApiReference();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<MicrowaveHub>("/microwaveHub");

app.MapControllers();

app.Run();

