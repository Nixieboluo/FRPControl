using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions.Handlers;

public interface IActionHandler<TPayload, TResponse>
    where TPayload : IActionPayload
{
    TResponse Handle(TPayload payload);
}
