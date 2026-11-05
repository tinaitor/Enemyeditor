using System.Windows;
using System.Windows.Media;

namespace ClickerGame
{
    public sealed class CDamageBoost : CCollectable
    {
        public CDamageBoost(Point position, double diameter, double lifetime)
            : base(position, diameter, lifetime, Brushes.OrangeRed, "Урон x2 на 10 секунд") { }

        public override string Description => "Урон x2 на 10 секунд";
        public override void OnClick(Player player) => player.ActivateDamageBoost(10);
    }
}
