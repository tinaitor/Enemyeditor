using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimedClicker
{
    // Нажимаемый объект (кружок). Знает только о себе:
    // где он, какого размера, сколько ему осталось жить и сколько очков он стоит.
    public class CObject
    {
        Point position;     // левый верхний угол объекта в сцене
        Size size;          // размер объекта
        double lifetime;    // оставшееся время жизни, сек
        Ellipse sprite;     // визуальное отображение
        double pointsValue; // очковая стоимость

        public CObject(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            sprite = new Ellipse();
            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            // чем меньше кружок и чем быстрее он исчезает — тем он дороже
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        // получение формы объекта
        public Ellipse getSprite() { return sprite; }

        // получение очковой стоимости
        public double getPointsValue() { return pointsValue; }

        // находится ли курсор внутри круга:
        // расстояние от курсора до центра <= радиуса
        public bool isMouseOnObject(Point mousePosition)
        {
            double radius = size.Width / 2;
            double centerX = position.X + radius;
            double centerY = position.Y + radius;
            double dx = mousePosition.X - centerX;
            double dy = mousePosition.Y - centerY;
            return dx * dx + dy * dy <= radius * radius;
        }

        // уменьшение времени жизни; true — время жизни истекло
        public bool updateLifetime(double deltaTime)
        {
            lifetime -= deltaTime;
            return lifetime <= 0;
        }
    }
}
