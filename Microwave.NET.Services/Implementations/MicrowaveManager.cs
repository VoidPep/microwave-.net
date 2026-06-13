using Microwave.NET.DataStructures.Constants;
using Microwave.NET.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Microwave.NET.Services.Implementations;

public class MicrowaveManager(IServiceProvider provider) : IMicrowaveManager
{
    public CancellationTokenSource Cts { get; set; } = new();

    public int? AddToTimerValue { get; set; } = null;
    public int? TimerInSeconds { get; set; } = null;
    public int? PowerLevel { get; set; } = null;
    public bool IsRunning { get; set; }
    public bool IsPaused { get; set; }
    public string Progress { get; set; } = "";
    public int RemainingTime { get; set; }

    public void SetPower(int powerLevel = 10)
    {
        if (IsRunning) return;

        PowerLevel = powerLevel;
    }

    public void SetTimerInSeconds(int timer)
    {
        if (IsRunning) return;

        TimerInSeconds = timer;
    }

    public void Start(out bool canStart)
    {
        Cts = new CancellationTokenSource();
        canStart = true;

        if (IsRunning && !IsPaused)
        {
            AddToTimerValue += 30;
            canStart = false;
            return;
        }

        if (TimerInSeconds == null || PowerLevel == null)
            QuickStart();

        IsRunning = true;
        IsPaused = false;
    }

    private void QuickStart()
    {
        TimerInSeconds ??= GlobalConstants.QuickStartTimerDefault;
        PowerLevel ??= GlobalConstants.QuickStartPowerDefault;
    }

    public void Stop()
    {
        Cts.Cancel();

        Reset();

        IsRunning = false;
        IsPaused = false;
    }

    private void Reset()
    {
        IsPaused = false;
        IsRunning = false;

        RemainingTime = 0;
        TimerInSeconds = 0;
    }

    public void Pause()
    {
        Cts.Cancel();

        IsPaused = true;
    }
}
