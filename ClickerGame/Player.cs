namespace ClickerGame
{
    public class Player
    {
        private const long StartDamage = 1;

        public BigNumber Gold { get; private set; }
        public BigNumber Damage { get; private set; }
        public int KillCount { get; private set; }

        public Player()
        {
            Gold = BigNumber.FromLong(0);
            Damage = BigNumber.FromLong(StartDamage);
            KillCount = 0;
        }

        public void AddGold(BigNumber amount)
        {
            Gold = Gold + amount;
        }

        public void OnEnemyKilled()
        {
            KillCount++;
        }
    }
}