
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.PresetsPrograms.Presets;

public class PresetFeijao: BasePresetedProgram, IPresetProgram
{
    public override string Nome { get => "Feijão congelado"; }
    public override string Instrucoes { get => "Feijão congelado"; }
    public override int Tempo { get => 480; }
    public override int Potencia { get => 9; }
    public override EnumAlimentos Alimento { get => EnumAlimentos.FEIJAO; }
}
