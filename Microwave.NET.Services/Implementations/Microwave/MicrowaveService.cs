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

        while (manager.RemainingTime < manager.TimerInSeconds)
        {
            try
            {
                bool flowControl = await ExecuteHeatingAsync();
                if (!flowControl)
                    break;

                manager.RemainingTime++;
            }
            catch (TaskCanceledException)
            {
                //
            }
        }

        await SetProgressAsync("\nAquecimento concluído.");
        await manager.StopAsync();
    }

    private async Task<bool> ExecuteHeatingAsync()
    {
        var cts = manager.Cts;

        if (cts.Token.IsCancellationRequested)
            return false;

        await SetProgressAsync(string.Concat(Enumerable.Repeat(manager.Character ?? '.', manager.PowerLevel ?? GlobalConstants.QuickStartPowerDefault)));
        await SetProgressAsync(" ");

        await Task.Delay(1000, cts.Token);

        return true;
    }

    private async Task SetProgressAsync(string progress)
    {
        manager.Progress += progress;

        await hubContext.Clients.All.SendAsync(
                "PropertyChanged",
                new MicrowaveStatusDto
                {
                    RemainingTime = manager.RemainingTime,
                    PowerLevel = manager.PowerLevel ?? GlobalConstants.QuickStartPowerDefault,
                    Progress = manager.Progress,
                    IsRunning = manager.IsRunning,
                    IsPaused = manager.IsPaused,
                }
            );
    }
}
