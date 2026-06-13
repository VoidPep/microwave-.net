using Microsoft.AspNetCore.SignalR;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.SignalR;
using Microwave.NET.Services.Interfaces;
using System.Timers;

namespace Microwave.NET.Services.Implementations;

public class MicrowaveService(IMicrowaveManager manager, IHubContext<MicrowaveHub> hubContext) : IMicrowaveService
{
    public async Task StartHeatingAsync()
    {
        manager.Progress = "";

        for (manager.RemainingTime = manager.RemainingTime; manager.RemainingTime < manager.TimerInSeconds; manager.RemainingTime++)
        {
            AddToTimer();

            bool flowControl = await ExecuteHeatingAsync();
            if (!flowControl)
                break;
        }

        await SetProgressAsync("Aquecimento concluído.");
    }

    private void AddToTimer()
    {
        if (!(manager.AddToTimerValue > 0))
            return;

        manager.TimerInSeconds += manager.AddToTimerValue.Value;
        manager.AddToTimerValue = 0;
    }

    private async Task<bool> ExecuteHeatingAsync()
    {
        var cts = manager.Cts;

        if (cts.Token.IsCancellationRequested)
            return false;

        await SetProgressAsync(string.Concat(Enumerable.Repeat('.', manager.PowerLevel ?? GlobalConstants.QuickStartPowerDefault)));
        await SetProgressAsync(" ");

        Thread.Sleep(1000);
        return true;
    }

    private async Task SetProgressAsync(string progress)
    {
        manager.Progress = progress;

        await hubContext.Clients.All.SendAsync(
                "TimerChanged",
                manager
            );
    }
}
