using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Enemyeditor
{
    public class CEnemyList
    {
        private List<EnemyTemplate> enemies = new List<EnemyTemplate>();

        public void Add(EnemyTemplate enemy)
        {
            enemies.Add(enemy);
        }

        public void Remove(EnemyTemplate enemy)
        {
            enemies.Remove(enemy);
        }

        public List<EnemyTemplate> GetAll()
        {
            return enemies;
        }

        public void SaveToJson(string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(enemies, options);
            File.WriteAllText(path, json);
        }

        public void LoadFromJson(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("JSON-файл не найден.", path);
            }

            string json = File.ReadAllText(path);

             
             
            List<EnemyTemplate> loaded =
                JsonSerializer.Deserialize<List<EnemyTemplate>>(json)
                ?? new List<EnemyTemplate>();

            enemies.Clear();
            enemies.AddRange(loaded);
        }
    }
}
