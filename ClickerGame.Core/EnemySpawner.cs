using System;
using System.Collections.Generic;
using System.Linq;

namespace ClickerGame.Core
{
    /// <summary>Хранит снимки шаблонов и выбирает их по нормализованным шансам.</summary>
    public sealed class EnemySpawner
    {
        private readonly EnemyTemplate[] templates;
        private readonly double[] normalizedChances;
        private readonly Random random;

        public EnemySpawner(IEnumerable<EnemyTemplate> source, Random? random = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            templates = source.Select(CopyTemplate).ToArray();
            if (templates.Length == 0)
                throw new ArgumentException("Нужен хотя бы один шаблон противника.", nameof(source));

            double maxChance = 0;
            foreach (EnemyTemplate template in templates)
            {
                if (template.Health <= 0 || template.Reward < 0 ||
                    template.Damage < 0 || template.Speed < 0 ||
                    template.HealthModifier < 1 || template.RewardModifier < 1 ||
                    template.DamageModifier < 1 || template.SpeedModifier < 1)
                    throw new ArgumentException("Характеристики и модификаторы шаблона некорректны.", nameof(source));
                if (double.IsNaN(template.SpawnChance) || double.IsInfinity(template.SpawnChance) || template.SpawnChance < 0)
                    throw new ArgumentException("Шанс появления должен быть конечным неотрицательным числом.", nameof(source));
                maxChance = Math.Max(maxChance, template.SpawnChance);
            }
            if (maxChance == 0)
                throw new ArgumentException("Хотя бы один шанс появления должен быть положительным.", nameof(source));

            // Деление на максимум исключает переполнение при суммировании весов.
            double sum = 0;
            foreach (EnemyTemplate template in templates)
                sum += template.SpawnChance / maxChance;
            normalizedChances = templates.Select(t => (t.SpawnChance / maxChance) / sum).ToArray();
            this.random = random ?? Random.Shared;
        }

        private static EnemyTemplate CopyTemplate(EnemyTemplate source)
        {
            ArgumentNullException.ThrowIfNull(source);
            return new EnemyTemplate
            {
                Name = source.Name ?? string.Empty,
                Icon = new EnemyIcon
                {
                    Name = source.Icon?.Name ?? string.Empty,
                    ImagePath = source.Icon?.ImagePath ?? string.Empty
                },
                Health = source.Health,
                Damage = source.Damage,
                Speed = source.Speed,
                Reward = source.Reward,
                SpawnChance = source.SpawnChance,
                HealthModifier = source.HealthModifier,
                DamageModifier = source.DamageModifier,
                SpeedModifier = source.SpeedModifier,
                RewardModifier = source.RewardModifier
            };
        }

        public Enemy Spawn(BigNumber victories)
        {
            ArgumentNullException.ThrowIfNull(victories);
            double roll = random.NextDouble();
            double cumulative = 0;
            int chosen = -1;
            for (int i = 0; i < templates.Length; i++)
            {
                if (normalizedChances[i] <= 0) continue;
                chosen = i;
                cumulative += normalizedChances[i];
                if (roll < cumulative) break;
            }

            EnemyTemplate template = templates[chosen];
            return new Enemy(template.Name, template.Icon.ImagePath,
                Scale(template.Health, template.HealthModifier, victories),
                Scale(template.Reward, template.RewardModifier, victories),
                Scale(template.Damage, template.DamageModifier, victories),
                Scale(template.Speed, template.SpeedModifier, victories));
        }

        // После n побед значение равно базовому значению * модификатор^n.
        private static BigNumber Scale(int initial, int modifier, BigNumber victories)
        {
            BigNumber result = new BigNumber(initial);
            if (initial == 0 || modifier == 1 || victories.IsZero) return result;

            BigNumber factor = new BigNumber(modifier);
            BigNumber exponent = victories;
            BigNumber two = new BigNumber(2);
            while (!exponent.IsZero)
            {
                BigNumber half = exponent / two;
                if (exponent != half * two) result = result * factor;
                exponent = half;
                if (!exponent.IsZero) factor = factor * factor;
            }
            return result;
        }
    }
}
