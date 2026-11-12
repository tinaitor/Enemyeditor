using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enemyeditor
{
    /// <summary>
    /// Полиморфная (де)сериализация CEnemyTemplate: при записи добавляет поле "$type",
    /// при чтении по нему выбирает нужный класс-наследник.
    /// </summary>
    public class EnemyTemplateConverter : JsonConverter<CEnemyTemplate>
    {
        public override CEnemyTemplate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader);
            string raw = jsonDoc.RootElement.GetRawText();

            // Старые файлы (до появления типов) не содержат "$type" — считаем их обычными противниками.
            if (jsonDoc.RootElement.ValueKind != JsonValueKind.Object)
                throw new JsonException("Ожидался объект противника.");

            string type = nameof(CNormalEnemyTemplate);
            if (jsonDoc.RootElement.TryGetProperty("$type", out JsonElement typeElement))
            {
                if (typeElement.ValueKind != JsonValueKind.String)
                    throw new JsonException("Поле $type должно быть строкой.");
                type = typeElement.GetString() ?? throw new JsonException("Поле $type не задано.");
            }

            CEnemyTemplate? result = type switch
            {
                nameof(CNormalEnemyTemplate) => JsonSerializer.Deserialize<CNormalEnemyTemplate>(raw, options),
                nameof(CArmoredEnemyTemplate) => JsonSerializer.Deserialize<CArmoredEnemyTemplate>(raw, options),
                nameof(CHealingEnemyTemplate) => JsonSerializer.Deserialize<CHealingEnemyTemplate>(raw, options),
                _ => throw new JsonException($"Неизвестный тип противника: {type}")
            };

            return result ?? throw new JsonException("Не удалось прочитать противника.");
        }

        public override void Write(Utf8JsonWriter writer, CEnemyTemplate value, JsonSerializerOptions options)
        {
            // Сериализуем по реальному типу объекта, чтобы попали и свойства наследника (Armor, HealChance).
            string json = JsonSerializer.Serialize(value, value.GetType(), options);
            using JsonDocument jsonDoc = JsonDocument.Parse(json);

            writer.WriteStartObject();
            writer.WriteString("$type", value.GetType().Name);
            foreach (JsonProperty property in jsonDoc.RootElement.EnumerateObject())
                property.WriteTo(writer);
            writer.WriteEndObject();
        }
    }
}
