using System;
using System.Collections.Generic;
using System.Windows;

namespace ClickerGame
{
    // A single polymorphic collection manages every type of sphere.
    public sealed class CController
    {
        private readonly List<CCollectable> objects = new();
        private readonly Random random = new();
        private double spawnElapsed;
        private const double SpawnInterval = 3;
        private const double SphereLifetime = 8;
        private const double SphereDiameter = 28;

        public IReadOnlyList<CCollectable> Objects => objects;

        public void Update(double delta, Size area)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
                if (objects[i].UpdateLifetime(delta)) objects.RemoveAt(i);

            if (area.Width < SphereDiameter || area.Height < SphereDiameter) return;
            spawnElapsed += delta;
            if (spawnElapsed < SpawnInterval || objects.Count >= 3) return;
            spawnElapsed = 0;

            Point position = new Point(
                random.NextDouble() * (area.Width - SphereDiameter),
                random.NextDouble() * (area.Height - SphereDiameter));
            CCollectable sphere = random.Next(3) switch
            {
                0 => new CDamageBoost(position, SphereDiameter, SphereLifetime),
                1 => new CCooldownReducer(position, SphereDiameter, SphereLifetime),
                _ => new CGoldBonus(position, SphereDiameter, SphereLifetime)
            };
            objects.Add(sphere);
        }

        public bool TryCollect(Point position, Player player)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (!objects[i].Contains(position)) continue;
                objects[i].OnClick(player);
                objects.RemoveAt(i);
                return true;
            }
            return false;
        }
    }
}
