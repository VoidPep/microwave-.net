using Microwave.NET.DataStructures.DTOs;

namespace Microwave.NET.Services.Interfaces;

public interface ICustomPresetService
{
    Task<List<PresetDto>> GetAllCustomPresetsAsync();
    Task<bool> CreatePresetAsync(CustomPresetDto preset);
    Task<bool> DeletePresetAsync(string nomPrograma);
    Task<bool> DeletePresetByIdAsync(int id);
    Task<List<PresetDto>> GetAllPresetsAsync();
    bool ValidateCharacter(char character, string? excludeName = null);
}
