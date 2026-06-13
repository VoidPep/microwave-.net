using Microwave.NET.Services.Implementations;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.API.Config;

public static class DependencyInjection
{
    public static void InjectAll(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        // Pensei em utilizar o signalR pois estou lidando com um singleton e uma aplicação web mais moderna, mas poderia ser feito com polling em um serviço de log
        // Porém eu não quis fazer um serviço de log sequer um arquivo de log para não encher mais o projeto e desacoplar ao máximo
        builder.Services.AddSignalR();

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
