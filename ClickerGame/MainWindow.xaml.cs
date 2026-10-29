using ClickerGame;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp48
{
    public partial class MainWindow : Window
    {
        private readonly EnemyRepository repository = new EnemyRepository();
        private readonly Player player = new Player();
        private Enemy? currentEnemy;

        public MainWindow()
        {
            InitializeComponent();
            repository.LoadFromJson("enemies.json");

            currentEnemy = CreateEnemyFromRandomTemplate();

            UpdateUI();
        }

        private Enemy? CreateEnemyFromRandomTemplate()
        {
            var template = repository.GetRandomTemplate();
            return template == null ? null : new Enemy(template, player.KillCount);
        }

        private void EnemyButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null) return;

            bool isDefeated = currentEnemy.TakeDamage(player.Damage);

            if (isDefeated)
            {
                player.AddGold(currentEnemy.Reward);
                player.OnEnemyKilled();                    // ← порядок важен
                currentEnemy = CreateEnemyFromRandomTemplate();
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
                string fixedPath = currentEnemy.ImagePath.Replace("/", "\\");
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fixedPath);

                if (File.Exists(fullPath))
                {
                    try
                    {
                        BitmapImage bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.UriSource = new Uri(fullPath, UriKind.Absolute);
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.EndInit();
                        EnemyImage.Source = bmp;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка загрузки картинки: " + ex.Message);
                    }
                }
            }
        }
    }
}