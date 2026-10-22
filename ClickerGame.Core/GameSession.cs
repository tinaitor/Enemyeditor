using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ClickerGame.Core
{
    /// <summary>Связывает игрока, текущего противника и последовательность боёв.</summary>
    public sealed class GameSession
    {
        private readonly EnemySpawner spawner;

        public Player Player { get; }
        public Enemy CurrentEnemy { get; private set; }
        public BigNumber VictoryCount { get; private set; } = BigNumber.Zero;

        public GameSession(IEnumerable<EnemyTemplate> templates, Random? random = null)
        {
            spawner = new EnemySpawner(templates, random);
            Player = new Player();
            CurrentEnemy = spawner.Spawn(VictoryCount);
        }

        public static GameSession LoadFromJson(string path, Random? random = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            string json = File.ReadAllText(path);
            List<EnemyTemplate> templates = JsonSerializer.Deserialize<List<EnemyTemplate>>(json)
                ?? new List<EnemyTemplate>();
            return new GameSession(templates, random);
        }

        // true: противник побеждён, награда выдана, следующий уже создан.
        public bool ClickEnemy()
        {
            if (!CurrentEnemy.TakeDamage(Player.Damage)) return false;

            Player.AddGold(CurrentEnemy.Reward);
            VictoryCount = VictoryCount + BigNumber.One;
            CurrentEnemy = spawner.Spawn(VictoryCount);
            return true;
        }

        public bool TryUpgradeDamage() => Player.TryUpgrade();
    }
}
