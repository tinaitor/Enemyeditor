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

            // Используем правильный оператор вычитания из BigNumber!
            Health = Health - damage;

            // Проверяем, не ушло ли здоровье в минус
            if (Health.ToString().StartsWith("-") || Health.ToString() == "0")
            {
                Health = BigNumber.FromLong(0);
                return true;
            }
            return false;
        }
    }
}