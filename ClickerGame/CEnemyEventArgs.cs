using System;

namespace ClickerGame
{
    public class CEnemyEventArgs : EventArgs
    {
        public IEnemy Enemy { get; }

        public CEnemyEventArgs(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}