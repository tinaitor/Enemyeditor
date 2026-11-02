using System.Windows;

namespace TimedClicker
{
    // Логика игры: создаёт, обновляет и удаляет объекты, считает очки и время.
    // Про отрисовку ничего не знает — окно само берёт у него список объектов.
    public class CController
    {
        List<CObject> objects;  // список собираемых объектов
        double spawnRate;       // время между созданием объектов, сек
        double time;            // время с момента создания последнего объекта
        Random rng;

        // минимальное и максимальное время жизни объектов, сек
        double minLifetime;
        double maxLifetime;

        // минимальный и максимальный размер объектов, px
        double minSpriteSize;
        double maxSpriteSize;

        Size sceneSize;         // размер зоны появления
        double points;          // набранные очки

        double gameDuration;    // сколько секунд объекты появляются
        double elapsed;         // сколько секунд прошло с начала игры

        public CController(double spawnRate, double gameDuration, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CObject>();
            this.spawnRate = spawnRate;
            this.gameDuration = gameDuration;
            this.sceneSize = sceneSize;
            time = spawnRate; // чтобы первый кружок появился сразу после старта
            elapsed = 0;
            points = 0;
            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 20;
            maxSpriteSize = 50;
        }

        // геттеры
        public List<CObject> getObjects() { return objects; }
        public double getPoints() { return points; }
        public double getTimeLeft() { return Math.Max(0, gameDuration - elapsed); }

        // время вышло и на поле ничего не осталось
        public bool isFinished() { return elapsed >= gameDuration && objects.Count == 0; }

        // создание нового объекта в случайной точке зоны
        void spawnObject()
        {
            double size = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);

            // вычитаем size, чтобы кружок целиком помещался в зону
            double x = rng.NextDouble() * Math.Max(0, sceneSize.Width - size);
            double y = rng.NextDouble() * Math.Max(0, sceneSize.Height - size);

            objects.Add(new CObject(new Point(x, y), size, lifetime));
        }

        // удаление объекта
        void removeObject(CObject obj)
        {
            objects.Remove(obj);
        }

        // обновление состояния; вызывается таймером окна
        public void update(double deltaTime)
        {
            elapsed += deltaTime;

            // новые объекты появляются, только пока не вышло время
            if (elapsed < gameDuration)
            {
                time += deltaTime;
                while (time >= spawnRate)
                {
                    spawnObject();
                    time -= spawnRate;
                }
            }

            // идём с конца, чтобы удаление не сбивало индексы
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].updateLifetime(deltaTime))
                    removeObject(objects[i]);
            }
        }

        // обработка клика; true — по объекту попали
        public bool click(Point mousePosition)
        {
            // с конца — чтобы при наложении сработал верхний (нарисованный последним) кружок
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
