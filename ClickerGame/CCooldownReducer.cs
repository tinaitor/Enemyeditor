using System.Windows;
using System.Windows.Media;

namespace ClickerGame
{
    public sealed class CCooldownReducer : CCollectable
    {
        public CCooldownReducer(Point position, double diameter, double lifetime)
            : base(position, diameter, lifetime, Brushes.DeepSkyBlue, "Перезарядка -0,1 с навсегда") { }

        public override string Description => "Перезарядка -0,1 с навсегда";
        public override void OnClick(Player player)
        {
            player.ApplyCooldownSphere();
        }

    }
}
