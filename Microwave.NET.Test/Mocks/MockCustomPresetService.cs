using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Test.Mocks;
public class MockCustomPresetService : ICustomPresetService
{
    public Task<bool> CreatePresetAsync(CustomPresetDto preset) => Task.FromResult(true);
    public Task<bool> DeletePresetByIdAsync(int id) => Task.FromResult(true);
    public Task<List<PresetDto>> GetAllCustomPresetsAsync() => Task.FromResult(new List<PresetDto>());
    public Task<List<PresetDto>> GetAllPresetsAsync() => Task.FromResult(new List<PresetDto>());
    public bool ValidateCharacter(char character, string? excludeId = null) => true;
    public Task<bool> DeletePresetAsync(string id) => Task.FromResult(true);
}
