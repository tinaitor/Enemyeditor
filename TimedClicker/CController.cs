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

        // События: контроллер только сообщает, что объект появился/исчез,
        // а как это показать на экране — решает подписчик (окно)
        public event ControllerEventHandler? addObject;
        public event ControllerEventHandler? removeObject;

        public CController(double spawnRate, double gameTime, double cooldown)
        {
            rng = new Random();
            objects = new List<CCollectable>();
            this.spawnRate = spawnRate;
            this.gameTime = gameTime;
            this.cooldown = cooldown;
            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 10;
            maxSpriteSize = 20;
            player = new CPlayer(cooldown);
        }

        // Начать новую игру: сбросить состояние и убрать оставшиеся объекты
        public void start(Size sceneSize)
        {
            this.sceneSize = sceneSize;
            for (int i = objects.Count - 1; i >= 0; i--)
                deleteObject(i);
            time = spawnRate;
            elapsedTime = 0;
            points = 0;
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

            CCollectable obj;
            double roll = rng.NextDouble();
            if (roll < 0.8)
                obj = new CPointGiver(pos, size, lifetime);
            else
                obj = new CCooldownReducer(pos, size, lifetime, 0.05);

            objects.Add(obj);
            addObject?.Invoke(this, new CControllerEventArgs(obj));
        }

        void deleteObject(int index)
        {
            CCollectable obj = objects[index];
            objects.RemoveAt(index);
            removeObject?.Invoke(this, new CControllerEventArgs(obj));
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
                    deleteObject(i);
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
                    deleteObject(i);
                    return true;
                }
            }
            return false;
        }
    }
}