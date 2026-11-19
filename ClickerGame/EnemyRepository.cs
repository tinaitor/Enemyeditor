using System;
using System.Collections.Generic;
using System.IO;

namespace ClickerGame
{
    public class EnemyRepository
    {
        private List<EnemyTemplate> templates = new();
        private static readonly Random rng = new();
        private readonly ISaveList<List<EnemyTemplate>> saver = new JsonEnemySaver();

        public void LoadFromJson(string path)
        {
            if (!File.Exists(path)) return;
            templates = saver.Load(path);
            NormalizeChances();
        }

        public List<EnemyTemplate> GetTemplates() => templates;

        public EnemyTemplate? GetRandomTemplate()
        {
            if (templates.Count == 0) return null;

            double total = 0;
            foreach (var t in templates) total += t.SpawnChance;
            if (total <= 0) return templates[rng.Next(templates.Count)];

            double roll = rng.NextDouble() * total;
            double acc = 0;
            foreach (var t in templates)
            {
                acc += t.SpawnChance;
                if (roll < acc) return t;
            }
            return templates[^1];
        }

        private void NormalizeChances()
        {
            if (templates.Count == 0) return;
            double sum = 0;
            foreach (var t in templates) sum += t.SpawnChance;
            if (sum <= 0) return;
            foreach (var t in templates) t.SpawnChance /= sum;
        }
    }
}