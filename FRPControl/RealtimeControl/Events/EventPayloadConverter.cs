using System.Text.Json;
using System.Text.Json.Serialization;
using FRPControl.RealtimeControl.Events.Payloads;

namespace FRPControl.RealtimeControl.Events;

public class EventPayloadConverter : JsonConverter<IEventPayload>
{
    // Deserializing is not needed currently.
    public override IEventPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, IEventPayload value, JsonSerializerOptions options)
    {
        // Serialize based on actual type, not the interface type.
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
