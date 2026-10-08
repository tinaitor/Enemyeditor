using System.Collections.Generic;

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
    }
}