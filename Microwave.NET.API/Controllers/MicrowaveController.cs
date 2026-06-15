using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microwave.NET.DataStructures.Constants;
using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.Services.Interfaces;


namespace Microwave.NET.API.Controllers;

[ApiController]
[Route("api/microwave")]
[Authorize]
public class MicrowaveController(IMicrowaveService microwaveService, IMicrowaveManager manager, ICustomPresetService customPresetService) : ControllerBase
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

    [HttpPost, Route("set-preset")]
    public async Task<IActionResult> SetPresetAsync(string nomePrograma)
    {
        try
        {
            await manager.SetPresetAsync(nomePrograma);
            return new JsonResult(new { Message = "Preset aplicado com sucesso." });
        }
        catch (Exception)
        {
            return new JsonResult(new { Message = "Erro ao aplicar o preset." });
        }
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

    [HttpGet, Route("presets")]
    public async Task<IActionResult> GetAllPresetsAsync()
    {
        try
        {
            var presets = await customPresetService.GetAllPresetsAsync();
            return new JsonResult(presets);
        }
        catch (Exception)
        {
            return new JsonResult(new { Message = "Erro ao carregar presets." });
        }
    }

    [HttpPost, Route("preset/create")]
    public async Task<IActionResult> CreatePresetAsync([FromBody] CustomPresetDto preset)
    {
        if (preset == null || string.IsNullOrWhiteSpace(preset.Nome) || 
            preset.Potencia <= 0 || preset.Tempo <= 0 || preset.Caractere == default)
            return new JsonResult(new { Message = "Por favor, preencha todos os campos obrigatórios." });

        if (!customPresetService.ValidateCharacter(preset.Caractere))
            return new JsonResult(new { Message = "O caractere de aquecimento já está em uso." });

        var success = await customPresetService.CreatePresetAsync(preset);

        if (success)
            return new JsonResult(new { Message = "Preset criado com sucesso." });

        return new JsonResult(new { Message = "Erro ao criar o preset." });
    }


    [HttpDelete, Route("preset/delete-by-id")]
    public async Task<IActionResult> DeletePresetByIdAsync(int id)
    {
        var success = await customPresetService.DeletePresetByIdAsync(id);

        if (success)
            return new JsonResult(new { Message = "Preset deletado com sucesso." });

        return new JsonResult(new { Message = "Erro ao deletar o preset." });
    }
}
