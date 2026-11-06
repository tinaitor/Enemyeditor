namespace ClickerGame
{
    public class Player
    {
        private const long StartDamage = 1;
        private const long StartUpgradeCost = 100;
        private const double DamageModifier = 1.2;
        private const double UpgradeModifier = 1.2;
        private const double InitialCooldown = 0.8;
        private const long StartCooldownUpgradeCost = 60;
        private const double MinimumCooldown = 0.2;
        private const int MaxBonusUpgradeLevel = 20;

        private readonly CCountdownTimer attackTimer =
            new CCountdownTimer(InitialCooldown);

        private double damageBoostLeft;

        public BigNumber Gold { get; private set; }
        public BigNumber Damage { get; private set; }
        public BigNumber UpgradeCost { get; private set; }
        public BigNumber CooldownUpgradeCost { get; private set; }

        public BigNumber GoldBonusUpgradeCost { get; private set; }
        public BigNumber CooldownSphereUpgradeCost { get; private set; }

        public int DamageLevel { get; private set; }
        public int KillCount { get; private set; }
        public int GoldBonusLevel { get; private set; }
        public int CooldownSphereLevel { get; private set; }

        public double Cooldown => attackTimer.Duration;
        public double CooldownLeft => attackTimer.Remaining;
        public bool CanAttack => attackTimer.IsReady;
        public double DamageBoostLeft => damageBoostLeft;

        public double CooldownSphereReduction =>
            0.1 + 0.05 * CooldownSphereLevel;

        public bool CanUpgradeCooldownSphere =>
            CooldownSphereLevel < MaxBonusUpgradeLevel &&
            Cooldown > MinimumCooldown + 0.000001;

        public BigNumber AttackDamage =>
            damageBoostLeft > 0 ? Damage.Multiply(2) : Damage;

        public Player()
        {
            Gold = BigNumber.FromLong(0);
            Damage = BigNumber.FromLong(StartDamage);
            UpgradeCost = BigNumber.FromLong(StartUpgradeCost);
            CooldownUpgradeCost = BigNumber.FromLong(StartCooldownUpgradeCost);

            GoldBonusUpgradeCost = BigNumber.FromLong(75);
            CooldownSphereUpgradeCost = BigNumber.FromLong(100);

            DamageLevel = 1;
            KillCount = 0;
            GoldBonusLevel = 0;
            CooldownSphereLevel = 0;
        }

        public void Update(double delta)
        {
            attackTimer.Update(delta);
            damageBoostLeft =
                System.Math.Max(0, damageBoostLeft - delta);
        }

        public bool TryAttack()
        {
            if (!CanAttack)
                return false;

            attackTimer.Start();
            return true;
        }

        public void ReduceCooldown(double seconds)
        {
            attackTimer.SetDuration(
                System.Math.Max(
                    MinimumCooldown,
                    System.Math.Round(Cooldown - seconds, 1)));
        }

        public void ActivateDamageBoost(double seconds)
        {
            damageBoostLeft =
                System.Math.Max(damageBoostLeft, seconds);
        }

        public void AddGold(BigNumber amount)
        {
            Gold = Gold + amount;
        }

        public void OnEnemyKilled()
        {
            KillCount++;
        }

        public BigNumber GetGoldSphereReward()
        {
            long baseReward = 50 + 10L * KillCount;
            double multiplier = System.Math.Pow(1.5, GoldBonusLevel);

            return BigNumber.FromLong(baseReward).Multiply(multiplier);
        }

        public void ApplyCooldownSphere()
        {
            ReduceCooldown(CooldownSphereReduction);
        }

        public bool TryUpgradeGoldSphere()
        {
            if (GoldBonusLevel >= MaxBonusUpgradeLevel ||
                Gold.CompareTo(GoldBonusUpgradeCost) < 0)
            {
                return false;
            }

            Gold = Gold - GoldBonusUpgradeCost;
            GoldBonusLevel++;
            GoldBonusUpgradeCost =
                GoldBonusUpgradeCost.Multiply(1.7);

            return true;
        }

        public bool TryUpgradeCooldownSphere()
        {
            if (!CanUpgradeCooldownSphere ||
                Gold.CompareTo(CooldownSphereUpgradeCost) < 0)
            {
                return false;
            }

            Gold = Gold - CooldownSphereUpgradeCost;
            CooldownSphereLevel++;
            CooldownSphereUpgradeCost =
                CooldownSphereUpgradeCost.Multiply(1.7);

            return true;
        }

        public bool TryUpgradeCooldown()
        {
            if (Cooldown <= MinimumCooldown + 0.000001 ||
                Gold.CompareTo(CooldownUpgradeCost) < 0)
            {
                return false;
            }

            Gold = Gold - CooldownUpgradeCost;
            ReduceCooldown(0.1);
            CooldownUpgradeCost =
                CooldownUpgradeCost.Multiply(1.5);

            return true;
        }

        public bool TryUpgrade()
        {
            if (Gold.CompareTo(UpgradeCost) < 0)
                return false;

            BigNumber increasedDamage = Damage.Multiply(DamageModifier);

            if (increasedDamage.CompareTo(Damage) <= 0)
                increasedDamage = Damage + BigNumber.FromLong(1);

            BigNumber nextCost =
                UpgradeCost.Multiply(UpgradeModifier * DamageLevel);

            Gold = Gold - UpgradeCost;
            Damage = increasedDamage;
            UpgradeCost = nextCost;
            DamageLevel++;

            return true;
        }
    }
}
