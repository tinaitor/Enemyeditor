using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClickerGame
{
    public class EnemyTemplateConverter : JsonConverter<EnemyTemplate>
    {
        public override EnemyTemplate Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
                throw new JsonException("Ожидался объект противника.");

            string raw = doc.RootElement.GetRawText();
            string type = nameof(CNormalEnemyTemplate);

            if (doc.RootElement.TryGetProperty("$type", out JsonElement typeEl))
            {
                if (typeEl.ValueKind != JsonValueKind.String)
                    throw new JsonException("Поле $type должно быть строкой.");
                type = typeEl.GetString() ?? throw new JsonException("Поле $type не задано.");
            }

            EnemyTemplate? result = type switch
            {
                nameof(CNormalEnemyTemplate) => JsonSerializer.Deserialize<CNormalEnemyTemplate>(raw, options),
                nameof(CArmoredEnemyTemplate) => JsonSerializer.Deserialize<CArmoredEnemyTemplate>(raw, options),
                nameof(CHealingEnemyTemplate) => JsonSerializer.Deserialize<CHealingEnemyTemplate>(raw, options),
                _ => throw new JsonException($"Неизвестный тип противника: {type}")
            };

            return result ?? throw new JsonException("Не удалось прочитать противника.");
        }

        public override void Write(Utf8JsonWriter writer, EnemyTemplate value,
            JsonSerializerOptions options)
        {
            string json = JsonSerializer.Serialize(value, value.GetType(), options);
            using JsonDocument doc = JsonDocument.Parse(json);
            writer.WriteStartObject();
            writer.WriteString("$type", value.GetType().Name);
            foreach (JsonProperty p in doc.RootElement.EnumerateObject())
                p.WriteTo(writer);
            writer.WriteEndObject();
        }
    }
}