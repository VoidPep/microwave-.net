using Microwave.NET.Services.Interfaces;
using System.Collections.Concurrent;

namespace Microwave.NET.Services.Implementations;

public class MicrowaveManager : IMicrowaveManager
{
    private readonly ConcurrentDictionary<string, CancellationTokenSource> operations = new();

    public void Start(string key, out CancellationTokenSource ct)
    {
        ct = new CancellationTokenSource();
        operations.TryAdd(key, ct);
    }

    public void Stop(string key)
    {
        if (operations.TryGetValue(key, out var cts))
            cts.Cancel();
    }
}
