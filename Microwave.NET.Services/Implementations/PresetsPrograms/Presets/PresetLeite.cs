
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.PresetsPrograms.Presets;

public class PresetLeite : BasePresetedProgram, IPresetProgram
{
    public override string Nome { get => "Leite"; }
    public override string Instrucoes { get => "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode causar fervura imediata causando risco de queimaduras."; }
    public override int Tempo { get => 300; }
    public override int Potencia { get => 5; }
    public override EnumAlimentos Alimento { get => EnumAlimentos.LEITE; }
    public override char Caractere { get => '+'; }
}
