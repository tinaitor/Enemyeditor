namespace ClickerGame
{
    public class Player
    {
        private const long StartDamage = 1;
        private const long StartUpgradeCost = 100;
        private const double DamageModifier = 1.2;
        private const double UpgradeModifier = 1.2;

        public BigNumber Gold { get; private set; }
        public BigNumber Damage { get; private set; }
        public BigNumber UpgradeCost { get; private set; }
        public int DamageLevel { get; private set; }
        public int KillCount { get; private set; }

        public Player()
        {
            Gold = BigNumber.FromLong(0);
            Damage = BigNumber.FromLong(StartDamage);
            UpgradeCost = BigNumber.FromLong(StartUpgradeCost);
            DamageLevel = 1;
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

        public bool TryUpgrade()
        {
            if (Gold.CompareTo(UpgradeCost) < 0) return false;

            BigNumber increasedDamage = Damage.Multiply(DamageModifier);
            if (increasedDamage.CompareTo(Damage) <= 0)
                increasedDamage = Damage + BigNumber.FromLong(1);

            BigNumber nextCost = UpgradeCost.Multiply(UpgradeModifier * DamageLevel);
            Gold = Gold - UpgradeCost;
            Damage = increasedDamage;
            UpgradeCost = nextCost;
            DamageLevel++;
            return true;
        }
    }
}
