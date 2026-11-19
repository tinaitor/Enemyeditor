using System;

namespace ClickerGame
{
    public class CHealingEnemy : CEnemy
    {
        public double HealChancePercent { get; }
        private readonly Random random = new();

        public CHealingEnemy(string name, BigNumber health, BigNumber gold,
            BigNumber reward, string imagePath, double healChancePercent)
            : base(name, health, gold, reward, imagePath)
        {
            HealChancePercent = Math.Clamp(healChancePercent, 0, 100);
        }

        public override void TakeDamage(BigNumber damage)
        {
            if (random.NextDouble() * 100 < HealChancePercent)
            {
                Health = Health + damage;
                return;
            }
            base.TakeDamage(damage);
        }
    }
}