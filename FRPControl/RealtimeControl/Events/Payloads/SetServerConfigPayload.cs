namespace FRPControl.RealtimeControl.Events.Payloads;

public record SetServerConfigPayload : IEventPayload
{
    public required string Config { get; init; }
}
