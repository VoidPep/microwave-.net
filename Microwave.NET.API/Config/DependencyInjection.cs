using Microwave.NET.API.Services;
using Microwave.NET.Services.Implementations.Microwave;
using Microwave.NET.Services.Implementations.PresetsPrograms.Presets;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.API.Config;

public static class DependencyInjection
{
    public static void InjectAll(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        // Pensei em utilizar o signalR pois estou lidando com um singleton e uma aplicação web mais moderna, mas poderia ser feito com polling em um serviço de log
        // Porém eu não quis fazer um serviço de log sequer um arquivo de log para não encher mais o projeto e desacoplar ao máximo
        builder.Services.AddSignalR();

        InjectServices(builder);
        InjectPresetPrograms(builder);

        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials());
        });
    }

    private static void InjectPresetPrograms(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPresetProgram, PresetCarneBovina>();
        builder.Services.AddTransient<IPresetProgram, PresetFeijao>();
        builder.Services.AddTransient<IPresetProgram, PresetFrango>();
        builder.Services.AddTransient<IPresetProgram, PresetLeite>();
        builder.Services.AddTransient<IPresetProgram, PresetPipoca>();
    }

    /// <summary>
    /// Um serviço scoped para controlar o microondas enquanto o outro é um serviço singleton (life cycle longo) para gerenciar o estado do microondas
    /// </summary>
    /// <param name="builder"></param>
    private static void InjectServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMicrowaveManager, MicrowaveManager>();
        builder.Services.AddScoped<IMicrowaveService, MicrowaveService>();
        builder.Services.AddTransient<ICustomPresetService, CustomPresetService>();
    }
}
