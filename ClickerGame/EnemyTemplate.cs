namespace ClickerGame
{
    public abstract class EnemyTemplate
    {
        public string Name { get; set; } = "unknown";
        public EnemyIcon Icon { get; set; } = new();
        public int Health { get; set; } = 100;
        public long GoldReward { get; set; }
        public double SpawnChance { get; set; } = 1.0;
        public double HealthModifier { get; set; } = 1.0;
        public double GoldModifier { get; set; } = 1.0;
    }

    public class CNormalEnemyTemplate : EnemyTemplate { }

    public class CArmoredEnemyTemplate : EnemyTemplate
    {
        public double Armor { get; set; } = 2;
    }

    public class CHealingEnemyTemplate : EnemyTemplate
    {
        public double HealChance { get; set; } = 10;
    }
}