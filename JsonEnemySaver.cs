using System.IO;
using System.Text.Json;

namespace Enemyeditor
{
    /// <summary>Сохранение списка противников в JSON с полиморфным конвертером.</summary>
    public class JsonEnemySaver : ISaveList<List<CEnemyTemplate>>
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new EnemyTemplateConverter() }
        };

        public List<CEnemyTemplate> Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("JSON-файл не найден.", path);

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<CEnemyTemplate>>(json, _options)
                   ?? new List<CEnemyTemplate>();
        }

        public void Save(List<CEnemyTemplate> data, string path)
        {
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }
    }
}
