using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions;

public record RequestMessage<TPayload>(ActionType ActionType, TPayload Payload)
    where TPayload : class, IActionPayload;
