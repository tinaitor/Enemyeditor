namespace ClickerGame.Core
{
    // Названия свойств совпадают с JSON редактора из Лабы 1.
    public sealed class EnemyTemplate
    {
        public string Name { get; set; } = string.Empty;
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Speed { get; set; }
        public int Reward { get; set; } = 1;
        public double SpawnChance { get; set; } = 1;
        public int HealthModifier { get; set; } = 1;
        public int DamageModifier { get; set; } = 1;
        public int SpeedModifier { get; set; } = 1;
        public int RewardModifier { get; set; } = 1;
    }
}
