using Microsoft.AspNetCore.SignalR;
using Microwave.NET.DataStructures.SignalR;

namespace Microwave.NET.Test.Mocks;

public class MockHubContext : IHubContext<MicrowaveHub>
{
    public IHubClients Clients => new MockHubClients();
    public IGroupManager Groups => throw new NotImplementedException();
}
