using System.IO;
using System.Text.Json;

namespace ClickerGame
{
    public class PlayerSaver : ISaveList<Player>
    {
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Converters = { new BigNumberConverter() }
        };

        public Player Load(string path)
        {
            if (!File.Exists(path)) return new Player();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Player>(json, _options) ?? new Player();
        }

        public void Save(Player data, string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(data, _options));
        }
    }
}