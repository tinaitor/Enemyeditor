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

        public Enemy(EnemyTemplate template, int level)
        {
            Name = template.Name;
            ImagePath = template.Icon?.ImagePath;

            MaxHealth = Scale(BigNumber.FromLong(template.Health), template.HealthModifier, level);
            Health = MaxHealth;

            Reward = Scale(BigNumber.FromLong(template.GoldReward), template.GoldModifier, level);
        }

        private static BigNumber Scale(BigNumber baseValue, double modifier, int level)
        {
            var result = baseValue;
            for (int i = 0; i < level; i++)
                result = result.Multiply(modifier);
            return result;
        }

        public bool TakeDamage(BigNumber damage)
        {
            if (IsDead) return false;

            string hpStr = Health.ToString();
            string dmgStr = damage.ToString();

            if (hpStr.Length < dmgStr.Length ||
                (hpStr.Length == dmgStr.Length && string.Compare(hpStr, dmgStr) <= 0))
            {
                Health = BigNumber.FromLong(0);
                return true;
            }

            Health = Health - damage;
            return false;
        }
    }
}