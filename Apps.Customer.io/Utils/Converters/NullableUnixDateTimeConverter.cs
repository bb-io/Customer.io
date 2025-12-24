using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Apps.Customer.io.Utils.Converters;

public class NullableUnixDateTimeConverter : UnixDateTimeConverter
{
    public override object? ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        if (reader.TokenType == JsonToken.Integer &&
            Convert.ToInt64(reader.Value) == 0)
            return null;

        return base.ReadJson(reader, objectType, existingValue, serializer);
    }
}
