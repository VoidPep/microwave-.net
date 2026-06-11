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
            return new JsonResult("Por favor informe um valor para o temporizador válido.");

        try
        {
            manager.SetTimerInSeconds(timeInSeconds);
        }
        catch (Exception)
        {

            throw;
        }

        return new JsonResult("Timer Set.");
    }

    [HttpPost, Route("set-power")]
    public IActionResult SetPower(int powerLevel = 10)
    {
        if (powerLevel < GlobalConstants.MinPowerLevel || powerLevel > GlobalConstants.MaxPowerLevel)
            return new JsonResult("Por favor informe um valor para a potência válido.");

        return new JsonResult("Power Set.");
    }

    [HttpPost, Route("start")]
    public IActionResult Start()
    {
        manager.Start(out CancellationTokenSource cts);

        microwaveService.Start(cts);

        return new JsonResult("Microwave started.");
    }

    [HttpPost, Route("cancel")]
    public IActionResult Cancel()
    {
        manager.Stop();

        return new JsonResult("Microwave canceled.");
    }
}
