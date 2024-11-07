using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions.Handlers;

public record ServerConfig(string Config);

public class GetServerConfigHandler : IActionHandler<GetServerConfigPayload, ServerConfig>
{
    public ServerConfig Handle(GetServerConfigPayload payload) => new("(toml frp config)");
}
