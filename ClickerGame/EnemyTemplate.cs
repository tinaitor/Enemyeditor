namespace ClickerGame
{
    public class EnemyTemplate
    {
        public string Name { get; set; } = "";
        public long Health { get; set; }
        public long Damage { get; set; }
        public long GoldReward { get; set; }


        
        public EnemyIcon Icon { get; set; } = new EnemyIcon();
    }
}
