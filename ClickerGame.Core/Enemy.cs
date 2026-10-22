using System;

namespace ClickerGame.Core
{
    /// <summary>Конкретный противник на сцене, созданный из шаблона.</summary>
    public sealed class Enemy
    {
        public string Name { get; }
        public string IconPath { get; }
        public BigNumber MaxHealth { get; private set; }
        public BigNumber CurrentHealth { get; private set; }
        public BigNumber Reward { get; private set; }
        public BigNumber Damage { get; private set; }
        public BigNumber Speed { get; private set; }
        public bool IsDefeated => CurrentHealth.IsZero;

        public Enemy(string name, string iconPath, BigNumber health,
            BigNumber reward, BigNumber damage, BigNumber speed)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(iconPath);
            ArgumentNullException.ThrowIfNull(health);
            ArgumentNullException.ThrowIfNull(reward);
            ArgumentNullException.ThrowIfNull(damage);
            ArgumentNullException.ThrowIfNull(speed);
            if (health.IsZero)
                throw new ArgumentOutOfRangeException(nameof(health), "Здоровье должно быть положительным.");

            Name = name;
            IconPath = iconPath;
            MaxHealth = health;
            CurrentHealth = health;
            Reward = reward;
            Damage = damage;
            Speed = speed;
        }

        // true возвращается только при ударе, который впервые победил противника.
        public bool TakeDamage(BigNumber amount)
        {
            ArgumentNullException.ThrowIfNull(amount);
            if (IsDefeated || amount.IsZero) return false;

            if (amount >= CurrentHealth)
            {
                CurrentHealth = BigNumber.Zero;
                return true;
            }

            CurrentHealth = CurrentHealth - amount;
            return false;
        }
    }
}
