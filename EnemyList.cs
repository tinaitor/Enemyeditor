using System.Collections.Generic;

namespace Enemyeditor
{
    public class CEnemyList
    {
        private List<CEnemyTemplate> enemies = new List<CEnemyTemplate>();

        // Работаем через интерфейс — список не знает, как именно сохраняются данные.
        private readonly ISaveList<List<CEnemyTemplate>> _serializer;

        public CEnemyList(ISaveList<List<CEnemyTemplate>>? serializer = null)
        {
            _serializer = serializer ?? new JsonEnemySaver();
        }

        public void Add(CEnemyTemplate enemy)
        {
            enemies.Add(enemy);
        }

        public void Remove(CEnemyTemplate enemy)
        {
            enemies.Remove(enemy);
        }

        public List<CEnemyTemplate> GetAll()
        {
            return enemies;
        }

        public void SaveToJson(string path)
        {
            _serializer.Save(enemies, path);
        }

        public void LoadFromJson(string path)
        {
            List<CEnemyTemplate> loaded = _serializer.Load(path);
            enemies.Clear();
            enemies.AddRange(loaded);
        }
    }
}
