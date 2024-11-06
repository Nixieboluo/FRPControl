using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions.Handlers;

public interface IMessageHandler<TPayload, TResponse>
    where TPayload : IActionPayload
{
    Task<TResponse> HandleMessageAsync(TPayload payload, CancellationToken cancellationToken);
}
