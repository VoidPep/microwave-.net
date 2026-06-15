
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.PresetsPrograms.Presets;

public class PresetCarneBovina: BasePresetedProgram, IPresetProgram
{
    public override string Nome { get => "Carne em pedaço ou fatias"; }
    public override string Instrucoes { get => "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."; }
    public override int Tempo { get => 840; }
    public override int Potencia { get => 4; }
    public override EnumAlimentos Alimento { get => EnumAlimentos.CARNE_BOI; }
}
