using Microsoft.AspNetCore.SignalR;

namespace Microwave.NET.Test.Mocks;

public class MockClientProxy : IClientProxy
{
    public Task SendAsync(string method, object?[]? args, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task SendCoreAsync(string method, object?[]? args, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
