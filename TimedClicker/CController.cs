using System.Windows;

namespace TimedClicker
{
    public class CController // класс, управляющий собираемыми объектами
    {
        List<CObject> objects; // список собираемых объектов
        double spawnRate;      // время между созданием собираемых объектов
        double time;           // время с момента создания последнего объекта
        Random rng;
        // минимальное и максимальное время жизни собираемых объектов
        double minLifetime;
        double maxLifetime;
        // минимальный и максимальный размер собираемых объектов
        double minSpriteSize;
        double maxSpriteSize;
        Size sceneSize;        // размер сцены
        double points;         // набранные очки
        double gameTime;       // отведённое время игры, сек
        double elapsedTime;    // время, прошедшее с начала игры

        public CController(double spawnRate, double startTime, double gameTime, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CObject>();
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

        // геттеры
        public List<CObject> getObjects() { return objects; }
        public double getPoints() { return points; }
        public double getTimeLeft() { return Math.Max(0, gameTime - elapsedTime); }

        // игра окончена: время вышло и на сцене не осталось объектов
        public bool isGameOver() { return elapsedTime >= gameTime && objects.Count == 0; }

        // создание нового объекта в случайной точке сцены
        void createObject()
        {
            double size = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);
            // объект должен целиком помещаться в сцену
            double x = rng.NextDouble() * (sceneSize.Width - size);
            double y = rng.NextDouble() * (sceneSize.Height - size);
            objects.Add(new CObject(new Point(x, y), size, lifetime));
        }

        // удаление объекта
        void removeObject(CObject obj)
        {
            objects.Remove(obj);
        }

        // обновление состояния: появление новых объектов и время жизни старых
        public void update(double deltaTime)
        {
            elapsedTime += deltaTime;

            // новые объекты появляются, пока не вышло отведённое время
            if (elapsedTime < gameTime)
            {
                time += deltaTime;
                if (time >= spawnRate)
                {
                    createObject();
                    time = 0;
                }
            }

            // проход с конца, чтобы удаление не сдвигало ещё не проверенные элементы
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].updateLifetime(deltaTime))
                    removeObject(objects[i]);
            }
        }

        // отслеживание нажатия курсора; возвращает true, если попали по объекту
        public bool click(Point mousePosition)
        {
            // с конца — при наложении срабатывает верхний объект
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
