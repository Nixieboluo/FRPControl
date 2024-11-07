using FRPControl.RealtimeControl.Actions.Handlers;
using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions;

public class ActionHandlerResolver
{
    private readonly Dictionary<Type, object> _handlerMapping;

    public ActionHandlerResolver()
    {
        _handlerMapping = new Dictionary<Type, object>();

        // Map payloads and handlers here.
        RegisterHandler<GetServerConfigHandler, GetServerConfigPayload, ServerConfig>();
    }

    private void RegisterHandler<THandler, TPayload, TResponse>()
        where TPayload : IActionPayload
        where THandler : IActionHandler<TPayload, TResponse> =>
        _handlerMapping[typeof(TPayload)] = Activator.CreateInstance<THandler>();

    public object? ResolveAndHandle(IActionPayload payload)
    {
        var payloadType = payload.GetType();

        if (_handlerMapping.TryGetValue(payloadType, out var handler))
        {
            // Get "Handle" method and execute.
            var handleMethod = handler.GetType().GetMethod("Handle");
            return handleMethod?.Invoke(handler, new object[] { payload });
        }

        throw new InvalidOperationException("No handler found for this payload.");
    }
}
