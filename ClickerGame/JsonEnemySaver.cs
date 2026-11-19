using System.IO;
using System.Text.Json;

namespace ClickerGame
{
    public class JsonEnemySaver : ISaveList<List<EnemyTemplate>>
    {
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Converters = { new EnemyTemplateConverter() }
        };

        public List<EnemyTemplate> Load(string path)
        {
            if (!File.Exists(path)) return new List<EnemyTemplate>();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<EnemyTemplate>>(json, _options)
                   ?? new List<EnemyTemplate>();
        }

        public void Save(List<EnemyTemplate> data, string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(data, _options));
        }
    }
}