using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerGame
{
    public class EnemyEventArgs : EventArgs
    {
        public BigNumber CurrentHealth { get; }

        public EnemyEventArgs(BigNumber currentHealth)
        {
            CurrentHealth = currentHealth;
        }
    }
}
