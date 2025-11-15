using System.Text.Json;
using System.Text.Json.Serialization;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Serialization.Converters
{
    public class JsonSizeConverter : JsonConverter<Size>
    {
        public override Size Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            float width = 0f;
            float height = 0f;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    if (propertyName.Equals("Width", StringComparison.OrdinalIgnoreCase))
                    {
                        width = reader.GetSingle();
                    }
                    else if (propertyName.Equals("Height", StringComparison.OrdinalIgnoreCase))
                    {
                        height = reader.GetSingle();
                    }
                }
            }

            return new Size(width, height);
        }

        public override void Write(Utf8JsonWriter writer, Size value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("Width", value.Width);
            writer.WriteNumber("Height", value.Height);
            writer.WriteEndObject();
        }
    }
}
