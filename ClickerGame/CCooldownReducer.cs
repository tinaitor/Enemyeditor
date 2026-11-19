using System.Windows;
using System.Windows.Media;

namespace ClickerGame
{
    public sealed class CCooldownReducer : CCollectable
    {
        public CCooldownReducer(Point position, double diameter, double lifetime)
            : base(position, diameter, lifetime, Brushes.DeepSkyBlue, "Скорость x2 на 10 секунд") { }

        public override string Description => "Скорость x2 на 10 секунд";

        public override void OnClick(Player player)
        {
            player.ActivateCooldownBoost(10);
        }
    }
}