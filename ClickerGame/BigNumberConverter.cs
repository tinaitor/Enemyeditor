using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClickerGame
{
    public class BigNumberConverter : JsonConverter<BigNumber>
    {
        public override BigNumber Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
            => new BigNumber(reader.GetString() ?? "0");

        public override void Write(Utf8JsonWriter writer, BigNumber value,
            JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}