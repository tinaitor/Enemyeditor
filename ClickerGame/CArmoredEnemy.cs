namespace ClickerGame
{
    public class CArmoredEnemy : CEnemy
    {
        public BigNumber Armor { get; }

        public CArmoredEnemy(string name, BigNumber health, BigNumber gold, BigNumber reward, string imagePath, BigNumber armor)
            : base(name, health, gold, reward, imagePath)
        {
            Armor = armor;
        }

        public override void TakeDamage(BigNumber damage)
        {
            BigNumber effective = damage - Armor;
            if (effective.CompareTo(BigNumber.Zero) <= 0)
                effective = new BigNumber("1");

            base.TakeDamage(effective);
        }
    }
}
