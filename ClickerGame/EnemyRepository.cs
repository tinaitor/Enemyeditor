using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace ClickerGame
{
    public class EnemyRepository
    {
        private List<EnemyTemplate> templates = new List<EnemyTemplate>();

        public void LoadFromJson(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            string jsonFromFile = File.ReadAllText(path);
            templates = JsonSerializer.Deserialize<List<EnemyTemplate>>(jsonFromFile) ?? new List<EnemyTemplate>();
        }
        public List<EnemyTemplate> GetTemplates()
        {
            return templates;
        }
    }
}
