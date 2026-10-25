using ClickerGame; 
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp48 
{
    public partial class MainWindow : Window
    {
        private readonly EnemyRepository repository = new EnemyRepository();
        private readonly Player player = new Player(BigNumber.FromLong(1)); 
        private Enemy? currentEnemy; 

        public MainWindow()
        {
            InitializeComponent();
            repository.LoadFromJson("enemies.json");

            
            var templates = repository.GetTemplates();
            if (templates != null && templates.Count > 0)
                currentEnemy = new Enemy(templates[0]);

            UpdateUI(); 
        }

       
        private void EnemyButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null) return;

            
            bool isDefeated = currentEnemy.TakeDamage(player.Damage);

           
            if (isDefeated)
            {
                
                player.AddGold(currentEnemy.Reward);

                
                var templates = repository.GetTemplates();
                if (templates != null && templates.Count > 0)
                    currentEnemy = new Enemy(templates[0]); 
            }

            
            UpdateUI();
        }

        
        private void UpdateUI()
        {
            if (currentEnemy == null)
            {
                EnemyNameText.Text = "Врагов нет";
                HealthText.Text = "HP: -";
                EnemyImage.Source = null;
                EnemyButton.IsEnabled = false;
                return;
            }

            EnemyButton.IsEnabled = true;
            EnemyNameText.Text = currentEnemy.Name;
            HealthText.Text = "Здоровье: " + currentEnemy.Health.ToString();

            GoldText.Text = "Золото: " + player.Gold.ToString();
            DamageText.Text = "Урон: " + player.Damage.ToString();

            
            if (!string.IsNullOrEmpty(currentEnemy.ImagePath))
            {
                
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, currentEnemy.ImagePath);
                if (File.Exists(fullPath))
                {
                    EnemyImage.Source = new BitmapImage(new Uri(fullPath));
                }
            }
        }
    }
}