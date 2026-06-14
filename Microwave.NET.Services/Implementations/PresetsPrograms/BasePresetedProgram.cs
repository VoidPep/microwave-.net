namespace Microwave.NET.Services.Implementations.PresetsPrograms;

public abstract class BasePresetedProgram
{
    public string Nome { get; set; }
    public string Instrucoes { get; set; }
    public int Tempo { get; set; }
    public int Potencia { get; set; }
    public EnumAlimentos Alimento { get; set; }

}
