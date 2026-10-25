namespace ClickerGame
{
    public class Player
    {
        public BigNumber Gold { get; private set; }
        public BigNumber Damage { get; private set; }

        public Player(BigNumber startDamage)
        {
            Gold = BigNumber.FromLong(0);
            Damage = startDamage;
        }

        public void AddGold(BigNumber amount)
        {
            Gold = Gold + amount;
        }
    }
}