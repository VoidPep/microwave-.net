using Microwave.NET.API.Config;
using Microwave.NET.DataStructures.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

DependencyInjection.InjectAll(builder);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHub<MicrowaveHub>("/microwaveHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

