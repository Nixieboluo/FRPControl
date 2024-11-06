using System.Text.Json;
using System.Text.Json.Serialization;
using FRPControl.RealtimeControl.Actions.Payloads;

namespace FRPControl.RealtimeControl.Actions;

public class RequestConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        // Read action type.
        if (!root.TryGetProperty("action", out var actionProperty))
            throw new JsonException("Missing required field: 'action'");

        var actionString = actionProperty.GetString();
        if (string.IsNullOrEmpty(actionString))
            throw new JsonException("Action field cannot be null or empty.");

        if (!Enum.TryParse(actionString, true, out ActionType action))
            throw new JsonException($"Unknown or unsupported action: {actionString}");

        // Deserialize payload
        if (!root.TryGetProperty("payload", out var payload))
            throw new JsonException("Missing required field: 'payload'");

        return action switch
        {
            ActionType.GetServerConfig => new RequestMessage<GetServerConfigPayload>(
                ActionType.GetServerConfig,
                JsonSerializer.Deserialize<GetServerConfigPayload>(payload.GetRawText(), options)
            ),
            _ => throw new JsonException($"Unknown or unsupported action: {actionString}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var message = (RequestMessage<IActionPayload>)value;
        var jsonObject = new { action = message.ActionType, payload = message.Payload };

        JsonSerializer.Serialize(writer, jsonObject, options);
    }
}
