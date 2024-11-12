using System.Net.WebSockets;
using System.Text;
using FRPControl.RealtimeControl.Events;
using FRPControl.RealtimeControl.Events.Payloads;

namespace FRPControl.RealtimeControl.Connection;

public class RealtimeControlContext
{
    public required WebSocket Connection { get; init; }

    public async Task SendEventAsync(IEventPayload payload)
    {
        var eventMessage = EventMessageEncoder.Encode(payload);
        await Connection.SendAsync(
            new ArraySegment<byte>(Encoding.UTF8.GetBytes(eventMessage)),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
    }
}
