using Apps.Customer.io.Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Customer.io.Utils.Converters;

public class EmailHeaderListConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<EmailHeader>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.String)
        {
            if (token.ToString() == "[]")
            {
                return new List<EmailHeader>();
            }
            
            return JsonConvert.DeserializeObject<List<EmailHeader>>(token.ToString())!;
        }

        if (token.Type == JTokenType.Array)
        {
            return token.ToObject<List<EmailHeader>>()!;
        }

        throw new JsonSerializationException("Unexpected token type for EmailHeader list");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}