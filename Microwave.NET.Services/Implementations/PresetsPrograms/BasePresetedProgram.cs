namespace Microwave.NET.Services.Implementations.PresetsPrograms;

public abstract class BasePresetedProgram
{
    public abstract string Nome { get; }
    public abstract string Instrucoes { get; }
    public abstract int Tempo { get; }
    public abstract int Potencia { get; }
    public abstract EnumAlimentos Alimento { get; }

}
