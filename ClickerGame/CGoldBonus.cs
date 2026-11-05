using System.Windows;
using System.Windows.Media;

namespace ClickerGame
{
    public sealed class CGoldBonus : CCollectable
    {
        public CGoldBonus(Point position, double diameter, double lifetime)
            : base(position, diameter, lifetime, Brushes.Gold, "Мгновенное золото") { }

        public override string Description => "Мгновенное золото";
        public override void OnClick(Player player) => player.AddGold(BigNumber.FromLong(50 + 10L * player.KillCount));
    }
}
