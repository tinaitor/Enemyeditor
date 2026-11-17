namespace ClickerGame
{
    public interface IEnemy
    {
        string Name { get; }
        BigNumber Health { get; }
        BigNumber Gold { get; }
        BigNumber Reward { get; }
        string ImagePath { get; }
        void TakeDamage(BigNumber damage);
    }


}
