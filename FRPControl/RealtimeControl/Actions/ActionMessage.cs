using System.Text.Json;
using System.Text.Json.Serialization;

namespace FRPControl.RealtimeControl.Actions;

public record ActionMessage
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActionType Action { get; init; }

    public JsonElement Payload { get; init; }
}
