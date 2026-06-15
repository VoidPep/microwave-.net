
using Microwave.NET.Services.Interfaces;

namespace Microwave.NET.Services.Implementations.PresetsPrograms.Presets;

public class PresetPipoca : BasePresetedProgram, IPresetProgram
{
    public override string Nome { get => "Pipoca"; }
    public override string Instrucoes { get => "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento."; }
    public override int Tempo { get => 180; }
    public override int Potencia { get => 7; }
    public override EnumAlimentos Alimento { get => EnumAlimentos.PIPOCA; }
}
