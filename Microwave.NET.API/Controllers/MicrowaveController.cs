using Microsoft.AspNetCore.Mvc;
using Microwave.NET.DataStructures.Enums;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.API.Controllers;

[Route("api/microwave")]
public class MicrowaveController(IMicrowaveService microwaveService, IMicrowaveManager manager) : ControllerBase
{
    [HttpPost, Route("set-timer")]
    public IActionResult SetTimer(int timeInSeconds)
    {
        try
        {
            microwaveService.SetTimerInSeconds(timeInSeconds);

        }
        catch (Exception)
        {

            throw;
        }

        return new JsonResult("Timer Set.");
    }

    [HttpPost, Route("set-power")]
    public IActionResult SetPower(PowerLevels powerLevel)
    {
        return new JsonResult("Power Set.");
    }

    [HttpPost, Route("start")]
    public IActionResult Start()
    {
        manager.Start("microwave-1", out CancellationTokenSource cts);

        microwaveService.Start(cts);

        return new JsonResult("Microwave started.");
    }

    [HttpPost, Route("cancel")]
    public IActionResult Cancel()
    {
        manager.Stop("microwave-1");

        return new JsonResult("Microwave canceled.");
    }
}
