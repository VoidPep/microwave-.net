using Microsoft.AspNetCore.Mvc;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.Services.Interfaces;


namespace Microwave.NET.API.Controllers;

[ApiController]
[Route("api/microwave")]
public class MicrowaveController(IMicrowaveService microwaveService, IMicrowaveManager manager) : ControllerBase
{

    [HttpPost, Route("set-timer")]
    public async Task<IActionResult> SetTimerAsync(int timeInSeconds)
    {
        if (timeInSeconds < GlobalConstants.MinTimerInSeconds || timeInSeconds > GlobalConstants.MaxTimerInSeconds)
            return new JsonResult(new { Message = "Por favor informe um valor para o temporizador válido." });

        try
        {
            await manager.SetTimerInSecondsAsync(timeInSeconds);
        }
        catch (Exception)
        {

            throw;
        }
        return new JsonResult(new { Message = "Temporizador alterado." });
    }

    [HttpPost, Route("set-power")]
    public async Task<IActionResult> SetPowerAsync(int powerLevel = 10)
    {
        if (powerLevel < GlobalConstants.MinPowerLevel || powerLevel > GlobalConstants.MaxPowerLevel)
            return new JsonResult(new { Message = "Por favor informe um valor para a potência válido." });

        try
        {
            await manager.SetPowerAsync(powerLevel);
        }
        catch (Exception)
        {

            throw;
        }
        return new JsonResult(new { Message = "Potência alterada." });
    } 

    [HttpPost, Route("start")]
    public async Task<IActionResult> StartAsync()
    {
        if(manager.IsPaused)
        {
            manager.IsPaused = false;
            return new JsonResult(new { Message = "Retornado" });
        }

        var canStart = await manager.StartAsync();

        if (canStart) await microwaveService.StartHeatingAsync();

        return new JsonResult(new { Message = "Iniciado" });
    }

    [HttpPost, Route("cancel")]
    public async Task<IActionResult> CancelAsync()
    {
        if (!manager.IsRunning)
        {
            manager.ResetSettings();
            return new JsonResult(new { Message = "Timer e potência limpos." });
        }

        if (!manager.IsPaused && manager.IsRunning)
        {
            await manager.PauseAsync();

            return new JsonResult(new { Message = "Pausado" });
        }

        if (manager.IsPaused)
        {
            await manager.StopAsync();

            return new JsonResult(new { Message = "Cancelado" });
        }

        return new JsonResult(new { Message = "" });
    }
}
