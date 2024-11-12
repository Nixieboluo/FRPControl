namespace FRPControl.RealtimeControl.Connection;

public class SessionManager
{
    private readonly Dictionary<string, RealtimeControlContext> _sessions = new();

    public void AddSession(string connectionId, RealtimeControlContext context)
    {
        _sessions.Add(connectionId, context);
    }

    public void RemoveSession(string connectionId)
    {
        _sessions.Remove(connectionId);
    }

    public RealtimeControlContext? GetSession(string connectionId)
    {
        return _sessions.GetValueOrDefault(connectionId);
    }
}
