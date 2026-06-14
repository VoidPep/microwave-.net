using Microwave.NET.Services.Implementations.PresetsPrograms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microwave.NET.Services.Interfaces;

/// <summary>
/// A ideia é fazer com que esse serviço seja responsável por gerenciar o estado do microondas em background, para não precisar travar a requisição
/// </summary>
public interface IMicrowaveManager
{
    CancellationTokenSource Cts { get; set; }

    int? TimerInSeconds { get; set; }
    int? PowerLevel { get; set; }
    bool IsRunning { get; set; }
    bool IsPaused { get; set; }
    string Progress { get; set; }
    int RemainingTime { get; set; }
    char? Character { get; set; }

    void ResetSettings();
    Task SetPowerAsync(int powerLevel = 10);
    Task SetTimerInSecondsAsync(int timer);
    Task<bool> StartAsync();
    Task StopAsync();
    Task PauseAsync();
    void SetPreset(EnumAlimentos opcao);
}
