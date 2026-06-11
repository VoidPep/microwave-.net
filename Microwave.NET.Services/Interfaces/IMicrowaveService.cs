using Microwave.NET.DataStructures.Enums;

namespace Microwave.NET.Services.Interfaces;

public interface IMicrowaveService
{
    void Start(CancellationTokenSource cts);
    void SetTimerInSeconds(int timer);
    void SetPower(PowerLevels powerLevel);
}
