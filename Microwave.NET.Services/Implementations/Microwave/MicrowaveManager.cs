using Microsoft.AspNetCore.SignalR;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.DataStructures.SignalR;
using Microwave.NET.Services.Implementations.PresetsPrograms;
using Microwave.NET.Services.Interfaces;
using System.Reflection;

namespace Microwave.NET.Services.Implementations.Microwave;

public class MicrowaveManager(IHubContext<MicrowaveHub> hubContext, IEnumerable<IPresetProgram> presetPrograms) : IMicrowaveManager
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

        if (TimerInSeconds == null || PowerLevel == null)
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
    }

    public async Task PauseAsync()
    {
        IsPaused = true;

        await UpdateHubAsync();
    }

    public void SetPreset(EnumAlimentos opcao)
    {
        var preset = presetPrograms.FirstOrDefault(q => q.Alimento == opcao);

        
    }

    private async Task UpdateHubAsync()
    {
        await hubContext.Clients.All.SendAsync(
            "PropertyChanged",
            new MicrowaveStatusDto
            {
                RemainingTime = RemainingTime,
                PowerLevel = PowerLevel ?? GlobalConstants.QuickStartPowerDefault,
                Progress = Progress,
                IsRunning = IsRunning,
                IsPaused = IsPaused,
            }
        );
    }

}
