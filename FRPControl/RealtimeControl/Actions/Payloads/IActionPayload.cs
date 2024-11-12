using FRPControl.RealtimeControl.Connection;

namespace FRPControl.RealtimeControl.Actions.Payloads;

public interface IActionPayload
{
    public Task Handle(RealtimeControlContext ctx);
}
