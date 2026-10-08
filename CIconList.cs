using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Enemyeditor
{
    public class CIconList
    {
        List<EnemyIcon> enemyIcons = new List<EnemyIcon>();
        public void LoadIconsFromFolder(string path)
        {
            string filter = "*.png";
            string[] files = Directory.GetFiles(path, filter);
            enemyIcons.Clear();
            foreach (string file in files)
            {
                
                enemyIcons.Add(new EnemyIcon
                {
                    Name = System.IO.Path.GetFileName(file),
                    ImagePath = file
                });
            }
        }
        public List<EnemyIcon> GetIcons()
        {
            return enemyIcons;
        }
    }
}
