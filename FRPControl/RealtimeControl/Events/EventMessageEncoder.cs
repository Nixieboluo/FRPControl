using System.Text.Json;
using FRPControl.RealtimeControl.Events.Payloads;

namespace FRPControl.RealtimeControl.Events;

public class EventMessageEncoder
{
    public static string Encode(IEventPayload payload)
    {
        var eventType = payload switch
        {
            SetServerConfigPayload => EventType.SetServerConfig,

            _ => throw new InvalidOperationException("Unknown event payload type, cannot serialize event message."),
        };

        return JsonSerializer.Serialize(new EventMessage { Event = eventType, Payload = payload });
    }
}
