using System.Text.Json;
using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions.Handlers;

public record ServerConfig(string Config);

public class GetServerConfigHandler : IMessageHandler<GetServerConfigPayload, ServerConfig>
{
    public Task<ServerConfig> HandleMessageAsync(GetServerConfigPayload payload, CancellationToken cancellationToken)
    {
        return Task.FromResult<ServerConfig>(new ServerConfig("(toml frp config)"));
    }
}
