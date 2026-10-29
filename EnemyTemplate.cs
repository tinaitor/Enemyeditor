namespace Enemyeditor
{
    public class EnemyTemplate
    {
        public string Name { get; set; } = string.Empty;
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
        public int Health { get; set; }
        public long GoldReward { get; set; }
        public double SpawnChance { get; set; } = 1.0;
        public double HealthModifier { get; set; } = 1.0;
        public double GoldModifier { get; set; } = 1.0;
    }
}