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

        public CController(double spawnRate, double startTime, double gameTime, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CCollectable>();
            this.spawnRate = spawnRate;
            time = startTime;
            this.gameTime = gameTime;
            this.sceneSize = sceneSize;
            elapsedTime = 0;
            points = 0;
            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 10;
            maxSpriteSize = 20;
        }

        public List<CCollectable> getObjects() { return objects; }
        public double getPoints() { return points; }
        public double getTimeLeft() { return Math.Max(0, gameTime - elapsedTime); }

        public bool isGameOver() { return elapsedTime >= gameTime && objects.Count == 0; }

        void createObject()
        {
            double size = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);

            double x = rng.NextDouble() * (sceneSize.Width - size);
            double y = rng.NextDouble() * (sceneSize.Height - size);
            objects.Add(new CCollectable(new Point(x, y), size, lifetime));
        }

        void removeObject(CCollectable obj)
        {
            objects.Remove(obj);
        }

        public void update(double deltaTime)
        {
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
                    removeObject(objects[i]);
            }
        }

        public bool click(Point mousePosition)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].isMouseOnObject(mousePosition))
                {
                    points += objects[i].getPointsValue();
                    removeObject(objects[i]);
                    return true;
                }
            }
            return false;
        }
    }
}
