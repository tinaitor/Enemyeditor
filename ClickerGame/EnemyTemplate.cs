namespace ClickerGame
{
    public class EnemyTemplate
    {
        public string Name { get; set; } = "";
        public long Health { get; set; }
        public double SpawnChance { get; set; } = 1.0;
        public long GoldReward { get; set; }
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
    }
}