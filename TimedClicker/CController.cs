using System;
using System.Collections.Generic;
using System.Windows;

namespace TimedClicker
{
    public class CController
    {
        List<CCollectable> objects;
        double spawnRate;
        double time;
        Random rng;
        double minLifetime;
        double maxLifetime;
        double minSpriteSize;
        double maxSpriteSize;
        Size sceneSize;
        double points;
        double gameTime;
        double elapsedTime;
        double cooldown;
        CPlayer player;

        public CController(double spawnRate, double startTime, double gameTime, double cooldown, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CCollectable>();
            this.spawnRate = spawnRate;
            time = startTime;
            this.gameTime = gameTime;
            this.cooldown = cooldown;
            this.sceneSize = sceneSize;
            elapsedTime = 0;
            points = 0;
            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 10;
            maxSpriteSize = 20;
            player = new CPlayer(cooldown);
        }

        public List<CCollectable> getObjects() { return objects; }
        public double getPoints() { return points; }
        public double getTimeLeft() { return Math.Max(0, gameTime - elapsedTime); }
        public bool isGameOver() { return elapsedTime >= gameTime && objects.Count == 0; }

        public double getCooldownLeft()
        {
            return player.getCooldownLeft();
        }

        public void pointsIncrease(double value)
        {
            points += value;
        }

        void createObject()
        {
            double size = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);
            double x = rng.NextDouble() * (sceneSize.Width - size);
            double y = rng.NextDouble() * (sceneSize.Height - size);
            Point pos = new Point(x, y);

            double roll = rng.NextDouble();
            if (roll < 0.8)
                objects.Add(new CPointGiver(pos, size, lifetime));
            else
                objects.Add(new CCooldownReducer(pos, size, lifetime, 0.05));
        }

        public void update(double deltaTime)
        {
            player.update(deltaTime);
            elapsedTime += deltaTime;

            if (elapsedTime < gameTime)
            {
                time += deltaTime;
                if (time >= spawnRate)
                {
                    createObject();
                    time = 0;
                }
            }

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].updateLifetime(deltaTime))
                    objects.RemoveAt(i);
            }
        }

        public bool click(Point mousePosition)
        {
            if (!player.CanClick) return false;

            player.startCooldown();

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].onClick(player, this, mousePosition))
                {
                    objects.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}