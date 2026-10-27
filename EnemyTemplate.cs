namespace Enemyeditor
{
    public class EnemyTemplate
    {
        public string Name { get; set; } = string.Empty;
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
        public int Health { get; set; }
        public int Damage { get; set; }
        public long GoldReward { get; set; }
    }
}
