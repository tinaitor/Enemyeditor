namespace ClickerGame
{
    public class EnemyTemplate
    {
        public string Name { get; set; } = "";
        public long Health { get; set; }
        public long Damage { get; set; }
        public int Speed { get; set; }

        // Вот это мы добавили:
        public long GoldReward { get; set; }

        // А вот эта строчка должна быть ТОЛЬКО ОДНА:
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
    }
}