namespace Enemyeditor
{
    public class EnemyTemplate
    {
        public string Name { get; set; } = string.Empty;
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Speed { get; set; }

        // Значения по умолчанию сохраняют совместимость с JSON из Лабы 1.
        public int Reward { get; set; } = 1;
        public double SpawnChance { get; set; } = 1;
        public int HealthModifier { get; set; } = 1;
        public int DamageModifier { get; set; } = 1;
        public int SpeedModifier { get; set; } = 1;
        public int RewardModifier { get; set; } = 1;
    }
}
