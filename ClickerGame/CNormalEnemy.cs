namespace ClickerGame
{
    public class CNormalEnemy : CEnemy
    {
        public CNormalEnemy(string name, BigNumber health, BigNumber gold, BigNumber reward, string imagePath)
            : base(name, health, gold, reward, imagePath) { }
    }
}
