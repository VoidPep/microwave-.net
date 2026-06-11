using Microwave.NET.Services.Implementations;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.API.Config;

public static class DependencyInjection
{
    public static void InjectAll(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        InjectServices(builder);
    }

    /// <summary>
    /// Um serviço scoped para controlar o microondas enquanto o outro é um serviço singleton (life cycle longo) para gerenciar o estado do microondas
    /// </summary>
    /// <param name="builder"></param>
    private static void InjectServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMicrowaveManager, MicrowaveManager>();
        builder.Services.AddScoped<IMicrowaveService, MicrowaveService>();
    }
}
