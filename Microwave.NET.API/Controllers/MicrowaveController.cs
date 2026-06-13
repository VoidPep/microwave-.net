using Microsoft.AspNetCore.Mvc;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.Services.Interfaces;


namespace Microwave.NET.API.Controllers;

[Route("api/microwave")]
public class MicrowaveController(IMicrowaveService microwaveService, IMicrowaveManager manager) : ControllerBase
{

    [HttpPost, Route("set-timer")]
    public IActionResult SetTimer(int timeInSeconds)
    {
        if (timeInSeconds < GlobalConstants.MinTimerInSeconds || timeInSeconds > GlobalConstants.MaxTimerInSeconds)
            return new JsonResult(new { Message = "Por favor informe um valor para o temporizador válido." });

        try
        {
            manager.SetTimerInSeconds(timeInSeconds);
        }
        catch (Exception)
        {

            throw;
        }
        return new JsonResult(new { Message = "Temporizador alterado." });
    }

    [HttpPost, Route("set-power")]
    public IActionResult SetPower(int powerLevel = 10)
    {
        if (powerLevel < GlobalConstants.MinPowerLevel || powerLevel > GlobalConstants.MaxPowerLevel)
            return new JsonResult(new { Message = "Por favor informe um valor para a potência válido." });

        return new JsonResult(new { Message = "Potência alterada." });
    }

    [HttpPost, Route("start")]
    public async Task<IActionResult> StartAsync()
    {
        manager.Start(out var canStart);

        if (canStart) await microwaveService.StartHeatingAsync();

        return new JsonResult(new { Message = "Iniciado" });
    }

    [HttpPost, Route("cancel")]
    public IActionResult Cancel()
    {
        manager.Stop();

        return new JsonResult(new { Message = "Cancelado" });
    }

    [HttpPost, Route("pause")]
    public IActionResult Pause()
    {
        manager.Pause();

        return new JsonResult(new { Message = "Pausado" });
    }
}
