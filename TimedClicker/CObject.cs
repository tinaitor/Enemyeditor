using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimedClicker
{
    public class CObject // нажимаемый объект (кружок)
    {
        Point position;     // позиция объекта в сцене (левый верхний угол)
        Size size;          // размер объекта
        double lifetime;    // оставшееся время жизни объекта, сек
        Ellipse sprite;     // визуальное отображение объекта
        double pointsValue; // очковая стоимость объекта

        public CObject(Point position, double size, double lifetime) // конструктор
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;
            // создание фигуры
            sprite = new Ellipse();
            // цвет фигуры
            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;
            // центрирование фигуры
            sprite.HorizontalAlignment = HorizontalAlignment.Center;
            sprite.VerticalAlignment = VerticalAlignment.Center;
            // размеры фигуры
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000; // очковая стоимость объекта
        }

        // получение формы объекта
        public Ellipse getSprite() { return sprite; }

        // получение очковой стоимости
        public double getPointsValue() { return pointsValue; }

        // проверка нахождения курсора мыши в пределах объекта:
        // расстояние от курсора до центра круга не больше радиуса
        public bool isMouseOnObject(Point mousePosition)
        {
            double radius = size.Width / 2;
            double dx = mousePosition.X - (position.X + radius);
            double dy = mousePosition.Y - (position.Y + radius);
            return dx * dx + dy * dy <= radius * radius;
        }

        // изменение времени жизни; возвращает true, если время жизни истекло
        public bool updateLifetime(double deltaTime)
        {
            lifetime -= deltaTime;
            return lifetime <= 0;
        }
    }
}
