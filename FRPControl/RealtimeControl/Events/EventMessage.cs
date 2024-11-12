using System.Text.Json.Serialization;
using FRPControl.RealtimeControl.Events.Payloads;

namespace FRPControl.RealtimeControl.Events;

public record EventMessage
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EventType Event { get; init; }

    [JsonConverter(typeof(EventPayloadConverter))]
    public IEventPayload Payload { get; init; }
}
