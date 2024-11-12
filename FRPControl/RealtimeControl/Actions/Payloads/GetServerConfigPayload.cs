using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FRPControl.RealtimeControl.Connection;
using FRPControl.RealtimeControl.Events.Payloads;

namespace FRPControl.RealtimeControl.Actions.Payloads;

public record GetServerConfigPayload : IActionPayload
{
    public async Task Handle(RealtimeControlContext ctx)
    {
        await ctx.SendEventAsync(
            new SetServerConfigPayload { Config = await File.ReadAllTextAsync("config/frps.toml") }
        );
    }
}
