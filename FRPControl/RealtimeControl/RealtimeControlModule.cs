using FRPControl.RealtimeControl.Connection;

namespace FRPControl.RealtimeControl;

public class RealtimeControlModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<SessionManager>();
    }
}
