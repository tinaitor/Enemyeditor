namespace ClickerGame
{
    public abstract class CEnemy : IEnemy
    {
        public string Name { get; }
        public BigNumber Health { get; private set; }
        public BigNumber Gold { get; }
        public BigNumber Reward { get; }
        public string ImagePath { get; }

        protected CEnemy(string name, BigNumber health, BigNumber gold, BigNumber reward, string imagePath)
        {
            Name = name;
            Health = health;
            Gold = gold;
            Reward = reward;
            ImagePath = imagePath;
        }

        public virtual void TakeDamage(BigNumber damage)
        {
            if (damage.CompareTo(BigNumber.Zero) <= 0) return;

            Health = Health - damage;
            if (Health.CompareTo(BigNumber.Zero) < 0)
                Health = BigNumber.Zero;
        }
    }
}
