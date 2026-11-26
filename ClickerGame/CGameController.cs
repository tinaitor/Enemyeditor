using System;

namespace ClickerGame
{
    public sealed class CGameController
    {
        private readonly EnemyRepository repository;
        private readonly Player player;
        private readonly Random random = new();

        private IEnemy? currentEnemy;

        public event EventHandler<CEnemyEventArgs>? EnemySpawned;
        public event EventHandler<CEnemyEventArgs>? EnemyDefeated;
        public event EventHandler<CEnemyEventArgs>? EnemyChanged;
        public event EventHandler<EnemyEventArgs> EnemyDamaged;

        public CGameController(EnemyRepository repository, Player player)
        {
            this.repository = repository;
            this.player = player;
        }

        public IEnemy? CurrentEnemy => currentEnemy;
        private void OnEnemyDamageTaken(object? sender, EnemyEventArgs e)
        {
            
            EnemyDamaged?.Invoke(this, e);
        }
        public void Start()
        {
            var enemy = SpawnFromRandomTemplate();
            currentEnemy = enemy;
            if (enemy != null)
                if (enemy is CEnemy cEnemy) cEnemy.DamageTaken += OnEnemyDamageTaken;
            EnemySpawned?.Invoke(this, new CEnemyEventArgs(enemy!));
        }

        public void Attack()
        {
            if (currentEnemy == null || !player.TryAttack()) return;

            bool doubleStrike = random.NextDouble() < player.DoubleStrikeChance;

            currentEnemy.TakeDamage(player.AttackDamage);
            bool dead = currentEnemy.Health.CompareTo(BigNumber.Zero) == 0;

            if (!dead && doubleStrike)
            {
                currentEnemy.TakeDamage(player.AttackDamage);
                dead = currentEnemy.Health.CompareTo(BigNumber.Zero) == 0;
            }

            if (!dead) return;

            var defeated = currentEnemy;
            player.AddGold(defeated.Reward);
            player.OnEnemyKilled();

            EnemyDefeated?.Invoke(this, new CEnemyEventArgs(defeated));

            var next = SpawnFromRandomTemplate();
            currentEnemy = next;
            if (next != null)
                if (next is CEnemy cNext) cNext.DamageTaken += OnEnemyDamageTaken;
            EnemyChanged?.Invoke(this, new CEnemyEventArgs(next!));
        }

        private IEnemy? SpawnFromRandomTemplate()
        {
            var template = repository.GetRandomTemplate();
            if (template == null) return null;

            int level = player.KillCount;
            BigNumber hp = EnemyUtils.Scale(
                BigNumber.FromLong(template.Health),
                template.HealthModifier, level);

            double rewardModifier = template.GoldModifier > 1.01
                ? template.GoldModifier
                : template.HealthModifier;

            BigNumber reward = EnemyUtils.Scale(
                BigNumber.FromLong(template.GoldReward),
                rewardModifier, level);

            BigNumber gold = reward;
            string icon = template.Icon?.ImagePath ?? "";

            try
            {
                return template switch
                {
                    CArmoredEnemyTemplate a => EnemyFactory.Create(
                        nameof(CArmoredEnemy),
                        a.Name, hp, gold, reward, icon,
                        BigNumber.FromLong((long)a.Armor)),

                    CHealingEnemyTemplate h => EnemyFactory.Create(
                        nameof(CHealingEnemy),
                        h.Name, hp, gold, reward, icon,
                        h.HealChance),

                    _ => EnemyFactory.Create(
                        nameof(CNormalEnemy),
                        template.Name, hp, gold, reward, icon)
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Не удалось создать противника: {ex.Message}");
                return null;
            }
        }
    }
}