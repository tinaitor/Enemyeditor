using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerGame
{
    // Adapted from TimedClicker: all bonus spheres share lifetime, appearance and hit testing.
    public abstract class CCollectable
    {
        private readonly Point position;
        private readonly double diameter;
        private double lifetime;

        public Ellipse Sprite { get; }

        protected CCollectable(Point position, double diameter, double lifetime, Brush color, string description)
        {
            this.position = position;
            this.diameter = diameter;
            this.lifetime = lifetime;
            Sprite = new Ellipse
            {
                Width = diameter,
                Height = diameter,
                Fill = color,
                Stroke = Brushes.White,
                StrokeThickness = 2,
                ToolTip = description
            };
            Canvas.SetLeft(Sprite, position.X);
            Canvas.SetTop(Sprite, position.Y);
        }

        public abstract string Description { get; }

        public bool UpdateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime <= 0;
        }

        public bool Contains(Point point)
        {
            double radius = diameter / 2;
            double dx = point.X - position.X - radius;
            double dy = point.Y - position.Y - radius;
            return dx * dx + dy * dy <= radius * radius;
        }

        public abstract void OnClick(Player player);
    }
}
