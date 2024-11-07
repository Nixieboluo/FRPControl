using System.Text.Json;
using System.Text.Json.Serialization;

namespace FRPControl.RealtimeControl.Actions;

public record ActionMessage
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActionType action { get; init; }

    public JsonElement payload { get; init; }
}
