using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WPFGameEngine.WPF.GE.Serialization.Converters
{
    public class JsonVector2Converter : JsonConverter<Vector2>
    {
        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteEndObject();
        }

        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected: '{{', but actual token was: {reader.TokenType}.");
            }

            float x = 0;
            float y = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new Vector2(x, y);
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString() ?? string.Empty;
                    reader.Read();

                    switch (propertyName.ToUpperInvariant())
                    {
                        case "X":
                            x = reader.GetSingle();
                            break;
                        case "Y":
                            y = reader.GetSingle();
                            break;
                    }
                }
            }

            throw new JsonException("Unable to Deserialize type Vector2!");
        }
    }
}
