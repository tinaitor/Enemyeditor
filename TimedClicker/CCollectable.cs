using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimedClicker
{
    public abstract class CCollectable
    {
        protected Point position;
        protected Size size;
        protected double lifetime;
        protected Ellipse sprite;

        public CCollectable(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            sprite = new Ellipse();
            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;
            sprite.HorizontalAlignment = HorizontalAlignment.Center;
            sprite.VerticalAlignment = VerticalAlignment.Center;
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
        }

        public Ellipse getSprite()
        {
            return sprite;
        }

        public bool isMouseOnObject(Point mousePosition)
        {
            double radius = size.Width / 2;
            double dx = mousePosition.X - (position.X + radius);
            double dy = mousePosition.Y - (position.Y + radius);
            return dx * dx + dy * dy <= radius * radius;
        }

        public bool updateLifetime(double deltaTime)
        {
            lifetime -= deltaTime;
            return lifetime <= 0;
        }

        public abstract bool onClick(CPlayer player, CController controller, Point mousePosition);
    }
}