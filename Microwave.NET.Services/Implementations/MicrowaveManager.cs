using Microwave.NET.DataStructures.Constants;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations;

public class MicrowaveManager : IMicrowaveManager
{
    private readonly CancellationTokenSource Cts = new();

    public int? TimerInSeconds { get; set; } = null;
    public int? PowerLevel { get; set; } = null;
    public bool IsRunning { get; set; }
    public bool IsPaused { get; set; }

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

    public void Start(out CancellationTokenSource ct)
    {
        ct = new CancellationTokenSource();

        if (IsRunning)
        {
            TimerInSeconds += 30;
            return;
        }

        if (TimerInSeconds == null || PowerLevel == null)
            QuickStart();

        IsRunning = true;
    }

    private void QuickStart()
    {
        TimerInSeconds ??= GlobalConstants.QuickStartTimerDefault;
        PowerLevel ??= GlobalConstants.QuickStartPowerDefault;
    }

    public void Stop()
    {
        Cts.Cancel();

        IsRunning = false;
    }

    public void Pause()
    {
        IsPaused = false;
    }
}
