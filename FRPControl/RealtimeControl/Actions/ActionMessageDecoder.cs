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

        return message.action switch
        {
            ActionType.GetServerConfig => JsonSerializer.Deserialize<GetServerConfigPayload>(
                message.payload.GetRawText()
            ) ?? throw new InvalidOperationException("Payload can not be null."),

            _ => throw new InvalidOperationException($"Unknown action type: {message.action}"),
        };
    }
}
