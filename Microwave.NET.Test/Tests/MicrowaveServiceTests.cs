using Microwave.NET.DataStructures.Constants;
using Microwave.NET.Services.Implementations.Microwave;
using Microwave.NET.Test.Mocks;

namespace Microwave.NET.Test.Tests;

public class MicrowaveServiceTests
{
    private MockMicrowaveManager _manager;
    private MicrowaveService _service;

    public MicrowaveServiceTests()
    {
        _manager = new MockMicrowaveManager();
        var hubContext = new MockHubContext();

        _service = new MicrowaveService(_manager, hubContext);
    }

    [Fact]
    public async Task StartHeatingAsync_DeveCompletarAquecimento()
    {
        // Arrange
        _manager.Progress = "";
        _manager.RemainingTime = 0;
        _manager.IsRunning = true;
        _manager.IsPaused = false;
        _manager.TimerInSeconds = 2;
        _manager.PowerLevel = 5;
        _manager.Character = '*';

        // Act
        await _service.StartHeatingAsync();

        // Assert
        Assert.NotEmpty(_manager.Progress);
        Assert.Contains("concluído", _manager.Progress);
    }

    [Fact]
    public async Task StartHeatingAsync_DeveIniciarQuickStark()
    {
        // Arrange
        _manager.Progress = "";
        _manager.RemainingTime = 0;
        _manager.IsRunning = true;
        _manager.IsPaused = false;
        _manager.TimerInSeconds = null;
        _manager.PowerLevel = null;
        _manager.Character = '.';

        // Act
        await _service.StartHeatingAsync();

        // Assert - Timer should have been set to QuickStart default
        Assert.Equal(GlobalConstants.QuickStartTimerDefault, _manager.TimerInSeconds);
        Assert.Equal(GlobalConstants.QuickStartPowerDefault, _manager.PowerLevel);
    }

    [Fact]
    public async Task StartHeatingAsync_DeveCancelarAquecimento()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        _manager.Progress = "";
        _manager.RemainingTime = 0;
        _manager.IsRunning = true;
        _manager.IsPaused = false;
        _manager.TimerInSeconds = 100;
        _manager.PowerLevel = 10;
        _manager.Character = '*';
        _manager.SetCts(cts);

        // Act
        var cancelTask = Task.Run(async () =>
        {
            await Task.Delay(50);
            cts.Cancel();
        });

        await _service.StartHeatingAsync();

        // Assert
        Assert.Contains("cancelado", _manager.Progress);
    }

    [Fact]
    public async Task StartHeatingAsync_DevePausarERetomarAquecimento()
    {
        // Arrange
        _manager.Progress = "";
        _manager.RemainingTime = 0;
        _manager.IsRunning = true;
        _manager.IsPaused = false;
        _manager.TimerInSeconds = 2;
        _manager.PowerLevel = 5;
        _manager.Character = '*';

        var pauseTask = Task.Run(async () =>
        {
            await Task.Delay(100);
            _manager.IsPaused = true;
            await Task.Delay(100);
            _manager.IsPaused = false;
        });

        // Act
        await _service.StartHeatingAsync();

        // Assert
        Assert.NotEmpty(_manager.Progress);
    }
}
