namespace Microwave.NET.DataStructures.DTOs;

public class PresetDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Potencia { get; set; }
    public char Caractere { get; set; }
    public int Tempo { get; set; }
    public string? Instrucoes { get; set; }
    public bool IsCustom { get; set; }
}
