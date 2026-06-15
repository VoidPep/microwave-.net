using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.PresetsPrograms;

public class CustomPresetProgram : BasePresetedProgram, IPresetProgram
{
    public CustomPresetProgram(string nome, EnumAlimentos alimento, int potencia, char caractere, int tempo, string? instrucoes = null)
    {
        _nome = nome;
        _alimento = alimento;
        _potencia = potencia;
        _caractere = caractere;
        _tempo = tempo;
        _instrucoes = instrucoes ?? string.Empty;
    }

    private readonly string _nome;
    private readonly EnumAlimentos _alimento;
    private readonly int _potencia;
    private readonly char _caractere;
    private readonly int _tempo;
    private readonly string _instrucoes;

    public override string Nome => _nome;
    public override string Instrucoes => _instrucoes;
    public override int Tempo => _tempo;
    public override int Potencia => _potencia;
    public override EnumAlimentos Alimento => _alimento;
    public override char Caractere => _caractere;
}
