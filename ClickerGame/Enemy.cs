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
          
            MaxHealth = BigNumber.FromLong(template.Health);
            Health = MaxHealth;
            
            Reward = BigNumber.FromLong(template.Speed);

            ImagePath = template.Icon?.ImagePath;
        }

        public bool TakeDamage(BigNumber damage)
        {
            if (IsDead) return false;

            
            string hpStr = Health.ToString();
            string dmgStr = damage.ToString();

            
            if (hpStr.Length < dmgStr.Length || (hpStr.Length == dmgStr.Length && string.Compare(hpStr, dmgStr) <= 0))
            {
                Health = BigNumber.FromLong(0);
                return true;
            }

            
            Health = Health - damage;
            return false;
        }
    }
}