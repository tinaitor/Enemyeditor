using System;

namespace ClickerGame.Core
{
    public sealed class Player
    {
        public BigNumber Gold { get; private set; } = BigNumber.Zero;
        public BigNumber Damage { get; private set; } = BigNumber.One;
        public BigNumber UpgradeCost { get; private set; } = new BigNumber(10);
        public BigNumber UpgradeLevel { get; private set; } = BigNumber.Zero;

        public void AddGold(BigNumber amount)
        {
            ArgumentNullException.ThrowIfNull(amount);
            Gold = Gold + amount;
        }

        public bool TryUpgrade()
        {
            if (Gold < UpgradeCost) return false;

            Gold = Gold - UpgradeCost;
            Damage = Damage * new BigNumber(2);
            UpgradeCost = UpgradeCost * new BigNumber(3);
            UpgradeLevel = UpgradeLevel + BigNumber.One;
            return true;
        }
    }
}
