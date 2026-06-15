using Microsoft.AspNetCore.SignalR;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.DataStructures.SignalR;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.Microwave;

public class MicrowaveService(IMicrowaveManager manager, IHubContext<MicrowaveHub> hubContext) : IMicrowaveService
{
    public async Task StartHeatingAsync()
    {
        manager.Progress = "";
        bool flowControl = true;

        manager.TimerInSeconds = manager.TimerInSeconds == 0 || manager.TimerInSeconds == null ? GlobalConstants.QuickStartTimerDefault : manager.TimerInSeconds;
        manager.PowerLevel = manager.PowerLevel == 0 || manager.PowerLevel == null ? GlobalConstants.QuickStartPowerDefault : manager.PowerLevel;

        while (manager.RemainingTime < manager.TimerInSeconds)
        {
            try
            {
                if (manager.IsPaused)
                {
                    await Task.Delay(500);
                    continue;
                }

                flowControl = await ExecuteHeatingAsync();
                if (!flowControl)
                    break;

                manager.RemainingTime++;
            }
            catch (TaskCanceledException)
            {
                await SetProgressAsync(" Aquecimento cancelado.");
                await manager.StopAsync();
                return;
            }
        }

        var status = flowControl ? "concluído" : "cancelado";
        await SetProgressAsync($" Aquecimento {status}.");
        await manager.StopAsync();
    }

    private async Task<bool> ExecuteHeatingAsync()
    {
        try
        {

            var cts = manager.Cts;

            if (cts.Token.IsCancellationRequested)
                return false;

            await SetProgressAsync(string.Concat(Enumerable.Repeat(manager.Character ?? '.', manager.PowerLevel ?? GlobalConstants.QuickStartPowerDefault)));
            await SetProgressAsync(" ");

            await Task.Delay(1000, cts.Token);

            return true;
        }
        catch (TaskCanceledException)
        {
            throw;
        }
    }

    private async Task SetProgressAsync(string progress)
    {
        manager.Progress += progress;

        await hubContext.Clients.All.SendAsync(
                "PropertyChanged",
                new MicrowaveStatusDto
                {
                    TotalTime = manager.TimerInSeconds,
                    RemainingTime = manager.RemainingTime,
                    PowerLevel = manager.PowerLevel ?? GlobalConstants.QuickStartPowerDefault,
                    Progress = manager.Progress,
                    IsRunning = manager.IsRunning,
                    IsPaused = manager.IsPaused,
                }
            );
    }
}
