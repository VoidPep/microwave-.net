using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.DataStructures.SignalR;
using Microwave.NET.Services.Implementations.PresetsPrograms;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.Microwave;

public class MicrowaveManager(IHubContext<MicrowaveHub> hubContext, IServiceScopeFactory scopeFactory, ICustomPresetService customPresetService) : IMicrowaveManager
{
    public CancellationTokenSource Cts { get; set; } = new();

    public int? TimerInSeconds { get; set; } = null;
    public int? PowerLevel { get; set; } = null;
    public bool IsRunning { get; set; }
    public bool IsPaused { get; set; }
    public string Progress { get; set; } = "";
    public int RemainingTime { get; set; }
    public char? Character { get; set; } = '.';

    public async Task SetPowerAsync(int powerLevel = 10)
    {
        if (IsRunning) return;

        PowerLevel = powerLevel;

        await UpdateHubAsync();
    }

    public async Task SetTimerInSecondsAsync(int timer)
    {
        if (IsRunning) return;

        TimerInSeconds = timer;

        await UpdateHubAsync();
    }

    public async Task<bool> StartAsync()
    {
        Cts = new CancellationTokenSource();
        bool canStart = true;

        if (IsRunning && !IsPaused)
        {
            TimerInSeconds += 30;
            return false;
        }

        if (TimerInSeconds == null || TimerInSeconds == 0 || PowerLevel == null || PowerLevel == 0)
            QuickStart();

        IsRunning = true;
        IsPaused = false;

        await UpdateHubAsync();
        return canStart;
    }

    private void QuickStart()
    {
        TimerInSeconds ??= GlobalConstants.QuickStartTimerDefault;
        PowerLevel ??= GlobalConstants.QuickStartPowerDefault;
    }

    public async Task StopAsync()
    {
        Cts.Cancel();

        Reset();

        IsRunning = false;
        IsPaused = false;

        await UpdateHubAsync();
    }

    public void ResetSettings()
    {
        PowerLevel = 0;
        TimerInSeconds = 0;
    }

    private void Reset()
    {
        IsPaused = false;
        IsRunning = false;

        RemainingTime = 0;
        TimerInSeconds = 0;
        Character = '.';
    }

    public async Task PauseAsync()
    {
        IsPaused = true;

        await UpdateHubAsync();
    }

    public async Task SetPresetAsync(string nomePrograma)
    {
        if (IsRunning) return;

        using var scope = scopeFactory.CreateScope();
        var presets = scope.ServiceProvider.GetServices<IPresetProgram>();

        var preset = presets.FirstOrDefault(p =>
            p is BasePresetedProgram bp && bp.Nome.Equals(nomePrograma, StringComparison.OrdinalIgnoreCase));

        if (preset is BasePresetedProgram basePreset)
        {
            ApplyPreset(basePreset);
            await UpdateHubAsync();
            return;
        }

        var customPresets = await customPresetService.GetAllCustomPresetsAsync();
        var customPreset = customPresets.FirstOrDefault(p => p.Nome.Equals(nomePrograma, StringComparison.OrdinalIgnoreCase));

        if (customPreset != null)
        {
            TimerInSeconds = customPreset.Tempo;
            PowerLevel = customPreset.Potencia;
            Character = customPreset.Caractere;

            await UpdateHubAsync();
        }
    }

    private void ApplyPreset(BasePresetedProgram preset)
    {
        TimerInSeconds = preset.Tempo;
        PowerLevel = preset.Potencia;
        Character = preset.Caractere;
    }

    private async Task UpdateHubAsync()
    {
        await hubContext.Clients.All.SendAsync(
            "PropertyChanged",
            new MicrowaveStatusDto
            {
                TotalTime = TimerInSeconds,
                RemainingTime = RemainingTime,
                PowerLevel = PowerLevel ?? GlobalConstants.QuickStartPowerDefault,
                Progress = Progress,
                IsRunning = IsRunning,
                IsPaused = IsPaused,
            }
        );
    }

}
