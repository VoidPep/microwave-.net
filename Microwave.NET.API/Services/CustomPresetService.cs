using Microsoft.AspNetCore.Hosting;
using Microwave.NET.DataStructures.DTOs;
using Microwave.NET.Services.Implementations.PresetsPrograms;
using Microwave.NET.Services.Interfaces;
using System.Text.Json;

namespace Microwave.NET.API.Services;

public class CustomPresetService(IEnumerable<IPresetProgram> presetPrograms, IWebHostEnvironment env) : ICustomPresetService
{
    private readonly string _customPresetsPath = Path.Combine(env.ContentRootPath, "Data", "custom_presets.json");
    private const char DefaultCharacter = '.';
    private const int PresetIdOffset = 10;

    public async Task<List<PresetDto>> GetAllCustomPresetsAsync()
    {
        var customPresets = await LoadCustomPresetsFromJsonAsync();
        return customPresets;
    }

    public async Task<List<PresetDto>> GetAllPresetsAsync()
    {
        var allPresets = new List<PresetDto>();

        int id = 1;
        foreach (var preset in presetPrograms)
        {
            if (preset is not BasePresetedProgram basePreset)
                continue;

            allPresets.Add(new PresetDto
            {
                Id = id++,
                Nome = basePreset.Nome,
                Potencia = basePreset.Potencia,
                Caractere = basePreset.Caractere,
                Tempo = basePreset.Tempo,
                Instrucoes = basePreset.Instrucoes,
                IsCustom = false
            });
        }

        var customPresets = await LoadCustomPresetsFromJsonAsync();
        allPresets.AddRange(customPresets);

        return allPresets;
    }

    public async Task<bool> CreatePresetAsync(CustomPresetDto preset)
    {
        if (string.IsNullOrWhiteSpace(preset.Nome) ||
            preset.Potencia <= 0 ||
            preset.Tempo <= 0 ||
            preset.Caractere == default)
        {
            return false;
        }

        if (!ValidateCharacter(preset.Caractere))
        {
            return false;
        }

        var customPresets = await LoadCustomPresetsFromJsonAsync();

        if (customPresets.Any(p => p.Id == preset.Id))
        {
            return false;
        }

        int newId = customPresets.Any() ? customPresets.Max(p => p.Id) + 1 : PresetIdOffset;

        var newPreset = new PresetDto
        {
            Id = newId,
            Nome = preset.Nome,
            Potencia = preset.Potencia,
            Caractere = preset.Caractere,
            Tempo = preset.Tempo,
            Instrucoes = preset.Instrucoes,
            IsCustom = true
        };

        customPresets.Add(newPreset);

        return await SaveCustomPresetsToJsonAsync(customPresets);
    }

    public async Task<bool> DeletePresetAsync(string nomePrograma)
    {
        var customPresets = await LoadCustomPresetsFromJsonAsync();
        var presetToRemove = customPresets.FirstOrDefault(p => p.Nome.Equals(nomePrograma, StringComparison.OrdinalIgnoreCase));

        if (presetToRemove == null)
            return false;

        customPresets.Remove(presetToRemove);

        return await SaveCustomPresetsToJsonAsync(customPresets);
    }

    public async Task<bool> DeletePresetByIdAsync(int id)
    {
        var customPresets = await LoadCustomPresetsFromJsonAsync();
        var presetToRemove = customPresets.FirstOrDefault(p => p.Id == id);

        if (presetToRemove == null)
            return false;

        customPresets.Remove(presetToRemove);

        return await SaveCustomPresetsToJsonAsync(customPresets);
    }

    public bool ValidateCharacter(char character, string? excludeName = null)
    {
        if (character == DefaultCharacter)
            return false;

        foreach (var preset in presetPrograms)
        {
            if (preset is BasePresetedProgram basePreset && basePreset.Caractere == character)
                return false;
        }

        var customPresetsTask = LoadCustomPresetsFromJsonAsync().Result;
        var existingCustom = customPresetsTask.FirstOrDefault(p => p.Caractere == character);

        if (existingCustom != null && (excludeName == null || !existingCustom.Nome.Equals(excludeName, StringComparison.OrdinalIgnoreCase)))
            return false;

        return true;
    }

    private async Task<List<PresetDto>> LoadCustomPresetsFromJsonAsync()
    {
        try
        {
            if (!File.Exists(_customPresetsPath))
                return new List<PresetDto>();

            var json = await File.ReadAllTextAsync(_customPresetsPath);
            var presets = JsonSerializer.Deserialize<List<PresetDto>>(json) ?? new List<PresetDto>();

            return presets;
        }
        catch
        {
            return new List<PresetDto>();
        }
    }

    private async Task<bool> SaveCustomPresetsToJsonAsync(List<PresetDto> presets)
    {
        try
        {
            var directory = Path.GetDirectoryName(_customPresetsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(presets, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_customPresetsPath, json);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
