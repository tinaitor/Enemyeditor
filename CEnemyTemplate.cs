using System.Text.Json.Serialization;

namespace Enemyeditor
{
    /// <summary>
    /// Абстрактный шаблон противника: общие для всех типов данные.
    /// Создать его напрямую нельзя — только через наследников.
    /// </summary>
    public abstract class CEnemyTemplate
    {
        string name = "unknown";
        public string Name
        {
            get { return name; }
            set { if (!string.IsNullOrWhiteSpace(value)) name = value.Trim(); else name = "unknown"; }
        }

        EnemyIcon icon = new EnemyIcon();
        public EnemyIcon Icon
        {
            get { return icon; }
            set { if (value != null) icon = value; else icon = new EnemyIcon(); }
        }

        int health = 100;
        public int Health
        {
            get { return health; }
            set { if (value > 0) health = value; else health = 100; }
        }

        long goldReward = 0;
        public long GoldReward
        {
            get { return goldReward; }
            set { if (value >= 0) goldReward = value; else goldReward = 0; }
        }

        double spawnChance = 1.0;
        public double SpawnChance
        {
            get { return spawnChance; }
            set { if (value > 0) spawnChance = value; else spawnChance = 1.0; }
        }

        double healthModifier = 1.0;
        public double HealthModifier
        {
            get { return healthModifier; }
            set { if (value > 0) healthModifier = value; else healthModifier = 1.0; }
        }

        double goldModifier = 1.0;
        public double GoldModifier
        {
            get { return goldModifier; }
            set { if (value > 0) goldModifier = value; else goldModifier = 1.0; }
        }

        /// <summary>Название типа для отображения в редакторе.</summary>
        [JsonIgnore]
        public abstract string TypeDisplayName { get; }

        /// <summary>Описание особого свойства типа (пустая строка, если его нет).</summary>
        [JsonIgnore]
        public abstract string SpecialDescription { get; }
    }

    /// <summary>Обычный противник — без особенностей.</summary>
    public class CNormalEnemyTemplate : CEnemyTemplate
    {
        [JsonIgnore] public override string TypeDisplayName => "Обычный";
        [JsonIgnore] public override string SpecialDescription => "";
    }

    /// <summary>Бронированный противник — броня уменьшает получаемый урон.</summary>
    public class CArmoredEnemyTemplate : CEnemyTemplate
    {
        double armor = 25;
        public double Armor
        {
            get { return armor; }
            set { if (value > 0) armor = value; else armor = 25; }
        }

        [JsonIgnore] public override string TypeDisplayName => "Бронированный";
        [JsonIgnore] public override string SpecialDescription => $"Броня: {Armor}";
    }

    /// <summary>Лечащийся противник — с некоторым шансом восстанавливает здоровье при получении урона.</summary>
    public class CHealingEnemyTemplate : CEnemyTemplate
    {
        double healChance = 10;
        /// <summary>Шанс лечения в процентах (больше 0 и не больше 100).</summary>
        public double HealChance
        {
            get { return healChance; }
            set { if (value > 0 && value <= 100) healChance = value; else healChance = 10; }
        }

        [JsonIgnore] public override string TypeDisplayName => "Лечащийся";
        [JsonIgnore] public override string SpecialDescription => $"Шанс лечения: {HealChance}%";
    }
}
