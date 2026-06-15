using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Test.Mocks;

public class MockMicrowaveManager : IMicrowaveManager
{
    private CancellationTokenSource _cts = new();

    public CancellationTokenSource Cts
    {
        get => _cts;
        set => _cts = value;
    }

    public int? TimerInSeconds { get; set; }
    public int? PowerLevel { get; set; }
    public bool IsRunning { get; set; }
    public bool IsPaused { get; set; }
    public string Progress { get; set; } = "";
    public int RemainingTime { get; set; }
    public char? Character { get; set; }

    public void SetCts(CancellationTokenSource cts) => _cts = cts;
    public void ResetSettings() { }
    public Task SetPowerAsync(int powerLevel = 10) => Task.CompletedTask;
    public Task SetTimerInSecondsAsync(int timer) => Task.CompletedTask;
    public Task<bool> StartAsync() => Task.FromResult(true);
    public Task StopAsync() => Task.CompletedTask;
    public Task PauseAsync() => Task.CompletedTask;
    public Task SetPresetAsync(string nomePrograma) => Task.CompletedTask;
}
