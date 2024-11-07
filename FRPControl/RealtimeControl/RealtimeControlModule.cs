using FRPControl.RealtimeControl.Actions;
using FRPControl.RealtimeControl.Actions.Handlers;
using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl;

public class RealtimeControlModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Action handlers
        services.AddTransient<IActionHandler<GetServerConfigPayload, ServerConfig>, GetServerConfigHandler>();

        // Handler factory
        services.AddTransient<ActionHandlerResolver>();
    }
}
