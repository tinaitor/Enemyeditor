using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClickerGame
{
    public class SphereEventArgs : EventArgs
    {
        public UIElement SphereUI { get; } // Ссылка на визуальный элемент сферы

        public SphereEventArgs(UIElement sphereUI)
        {
            SphereUI = sphereUI;
        }
    }
}
