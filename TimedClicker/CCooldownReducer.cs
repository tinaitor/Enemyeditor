using System.Windows;
using System.Windows.Media;

namespace TimedClicker
{
    public class CCooldownReducer : CCollectable
    {
        private double speedBonus;

        public CCooldownReducer(Point position, double size, double lifetime, double speedBonus)
            : base(position, size, lifetime)
        {
            sprite.Fill = Brushes.Orange;
            this.speedBonus = speedBonus;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (!isMouseOnObject(mousePosition))
                return false;

            player.increaseSpeed(speedBonus);
            return true;
        }
    }
}