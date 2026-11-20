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
            BigNumber effective;
            if (damage.CompareTo(Armor) >= 0)
            {
                effective = damage - Armor;
            }
            else
            {
                effective = new BigNumber("1");
            }
            base.TakeDamage(effective);

        }
    }
}
