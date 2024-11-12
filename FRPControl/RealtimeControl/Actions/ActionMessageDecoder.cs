using System.Text.Json;
using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions;

public class ActionMessageDecoder
{
    public static IActionPayload Decode(string json)
    {
        var message = JsonSerializer.Deserialize<ActionMessage>(json);
        if (message is null)
            throw new InvalidOperationException("Cannot deserialize message.");

        var payload = message.Action switch
        {
            ActionType.GetServerConfig => JsonSerializer.Deserialize<GetServerConfigPayload>(
                message.Payload.GetRawText()
            ),

            _ => throw new InvalidOperationException($"Unknown action type: {message.Action}"),
        };

        if (payload is null)
            throw new InvalidOperationException("Payload can not be null.");

        return payload;
    }
}
