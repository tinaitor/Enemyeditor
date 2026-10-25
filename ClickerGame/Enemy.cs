namespace ClickerGame
{
    public class Enemy
    {
        public string Name { get; private set; }
        public BigNumber MaxHealth { get; private set; }
        public BigNumber Health { get; private set; }
        public BigNumber Reward { get; private set; }
        public string ImagePath { get; private set; }
        public bool IsDead => Health.ToString() == "0";

        public Enemy(EnemyTemplate template)
        {
            Name = template.Name;
            // Берем здоровье из шаблона
            MaxHealth = BigNumber.FromLong(template.Health);
            Health = MaxHealth;
            // В качестве награды пока берем то же здоровье (или поменяй, если в JSON есть GoldReward)
            Reward = BigNumber.FromLong(template.Health);
            // Достаем путь к картинке
            ImagePath = template.Icon?.ImagePath;
        }

        public bool TakeDamage(BigNumber damage)
        {
            if (IsDead) return false;

            // Вычитаем урон (упрощенно, для работы клика)
            long currentHp = long.Parse(Health.ToString());
            long dmg = long.Parse(damage.ToString());
            long newHp = currentHp - dmg;
            if (newHp < 0) newHp = 0;

            Health = BigNumber.FromLong(newHp);

            return Health.ToString() == "0"; // Возвращаем true, если умер
        }
    }
}