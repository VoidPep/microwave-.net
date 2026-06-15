using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.SignalR;
using Microwave.NET.Services.Implementations.Microwave;
using Microwave.NET.Services.Interfaces;
using Microwave.NET.Test.Mocks;

namespace Microwave.NET.Test.Tests;

public class MicrowaveManagerTests
{
    private Mock<IServiceScopeFactory> _scopeFactory;
    private MicrowaveManager _manager;
    private MockHubContext _hubContext;
    private MockCustomPresetService _customPresetService;


    public MicrowaveManagerTests()
    {
        _scopeFactory = new Mock<IServiceScopeFactory>();
        _hubContext = new MockHubContext();
        _customPresetService = new MockCustomPresetService();

        _manager = new MicrowaveManager(_hubContext, _scopeFactory.Object, _customPresetService);
    }

    [Fact]
    public async Task SetTimerInSecondsAsync_DeveSetarTimer_QuandoEstaParado()
    {
        // Arrange
        int timerValue = 60;

        // Act
        await _manager.SetTimerInSecondsAsync(timerValue);

        // Assert
        Assert.Equal(timerValue, _manager.TimerInSeconds);
        Assert.False(_manager.IsRunning);
    }

    [Fact]
    public async Task SetTimerInSecondsAsync_NaoDeveSetarTimer_QuandoEstaExecutando()
    {
        // Arrange
        int initialTimer = 30;
        _manager.TimerInSeconds = initialTimer;
        _manager.IsRunning = true;

        // Act
        await _manager.SetTimerInSecondsAsync(60);

        // Assert
        Assert.Equal(initialTimer, _manager.TimerInSeconds);
    }

    [Fact]
    public async Task SetPowerAsync_DeveSetarPotencia_QuandoEstaParado()
    {
        // Arrange
        int powerLevel = 5;

        // Act
        await _manager.SetPowerAsync(powerLevel);

        // Assert
        Assert.Equal(powerLevel, _manager.PowerLevel);
        Assert.False(_manager.IsRunning);
    }

    [Fact]
    public async Task SetPowerAsync_DeveSetarPotencia_QuandoEstaRodando()
    {
        // Arrange
        int initialPower = 7;
        _manager.PowerLevel = initialPower;
        _manager.IsRunning = true;

        // Act
        await _manager.SetPowerAsync(10);

        // Assert
        Assert.Equal(initialPower, _manager.PowerLevel);
    }

    [Fact]
    public async Task StartAsync_DeveIniciarAquencimento()
    {
        // Arrange
        _manager.TimerInSeconds = 30;
        _manager.PowerLevel = 10;

        // Act
        bool result = await _manager.StartAsync();

        // Assert
        Assert.True(result);
        Assert.True(_manager.IsRunning);
        Assert.False(_manager.IsPaused);
    }

    [Fact]
    public async Task StartAsync_DeveIniciar_ComQuickStart()
    {
        // Arrange
        _manager.TimerInSeconds = null;
        _manager.PowerLevel = null;

        // Act
        await _manager.StartAsync();

        // Assert
        Assert.Equal(GlobalConstants.QuickStartTimerDefault, _manager.TimerInSeconds);
        Assert.Equal(GlobalConstants.QuickStartPowerDefault, _manager.PowerLevel);
        Assert.True(_manager.IsRunning);
    }

    [Fact]
    public async Task PauseAsync_DevePausarAquecimento()
    {
        // Arrange
        _manager.IsRunning = true;
        _manager.IsPaused = false;

        // Act
        await _manager.PauseAsync();

        // Assert
        Assert.True(_manager.IsPaused);
    }

    [Fact]
    public async Task StopAsync_DevePararEResetar()
    {
        // Arrange
        _manager.IsRunning = true;
        _manager.IsPaused = true;
        _manager.TimerInSeconds = 60;
        _manager.RemainingTime = 30;

        // Act
        await _manager.StopAsync();

        // Assert
        Assert.False(_manager.IsRunning);
        Assert.False(_manager.IsPaused);
        Assert.Equal(0, _manager.RemainingTime);
    }
}
