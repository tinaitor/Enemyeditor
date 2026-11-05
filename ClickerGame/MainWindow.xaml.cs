using ClickerGame;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfApp48
{
    public partial class MainWindow : Window
    {
        private readonly EnemyRepository repository = new EnemyRepository();
        private readonly Player player = new Player();
        private readonly CController bonuses = new CController();
        private readonly DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private TimeSpan previousTick;
        private Enemy? currentEnemy;
        private Enemy? displayedEnemy;

        public MainWindow()
        {
            InitializeComponent();
            repository.LoadFromJson(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "enemies.json"));
            currentEnemy = CreateEnemyFromRandomTemplate();
            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateUI();
        }

        private Enemy? CreateEnemyFromRandomTemplate()
        {
            var template = repository.GetRandomTemplate();
            return template == null ? null : new Enemy(template, player.KillCount);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeSpan now = clock.Elapsed;
            double delta = Math.Min(0.25, (now - previousTick).TotalSeconds);
            previousTick = now;
            player.Update(delta);
            if (currentEnemy != null)
                bonuses.Update(delta, new Size(BonusCanvas.ActualWidth, BonusCanvas.ActualHeight));
            UpdateUI();
        }

        private void GameArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BonusCanvas);
            if (bonuses.TryCollect(point, player))
            {
                UpdateUI();
                return;
            }

            if (currentEnemy == null || !player.TryAttack()) return;
            bool isDefeated = currentEnemy.TakeDamage(player.AttackDamage);
            if (isDefeated)
            {
                player.AddGold(currentEnemy.Reward);
                player.OnEnemyKilled();
                currentEnemy = CreateEnemyFromRandomTemplate();
            }
            UpdateUI();
        }

        private void UpgradeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.TryUpgrade()) UpdateUI();
        }

        private void UpgradeCooldownButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.TryUpgradeCooldown()) UpdateUI();
        }

        private void UpdateUI()
        {
            GoldText.Text = "Золото: " + player.Gold;
            DamageText.Text = "Урон: " + player.AttackDamage;
            CooldownText.Text = player.CanAttack
                ? $"Атака готова (перезарядка {player.Cooldown:F1} с)"
                : $"Перезарядка: {player.CooldownLeft:F1} с";
            BoostText.Text = player.DamageBoostLeft > 0
                ? $"Усиление урона: {player.DamageBoostLeft:F1} с"
                : "Усиление урона: нет";
            UpgradeDamageButton.Content = "Улучшить урон (" + player.UpgradeCost + " золота)";
            UpgradeDamageButton.IsEnabled = player.Gold.CompareTo(player.UpgradeCost) >= 0;
            UpgradeCooldownButton.Content = "Ускорить атаку (" + player.CooldownUpgradeCost + " золота)";
            UpgradeCooldownButton.IsEnabled = player.Cooldown > 0.200001 && player.Gold.CompareTo(player.CooldownUpgradeCost) >= 0;

            BonusCanvas.Children.Clear();
            foreach (CCollectable sphere in bonuses.Objects)
                BonusCanvas.Children.Add(sphere.Sprite);

            if (currentEnemy == null)
            {
                displayedEnemy = null;
                EnemyNameText.Text = "Врагов нет";
                HealthText.Text = "HP: -";
                EnemyImage.Source = null;
                return;
            }

            EnemyNameText.Text = currentEnemy.Name;
            HealthText.Text = "Здоровье: " + currentEnemy.Health;
            if (ReferenceEquals(displayedEnemy, currentEnemy)) return;
            displayedEnemy = currentEnemy;
            string? fullPath = ResolveImagePath(currentEnemy.ImagePath);
            if (fullPath == null) { EnemyImage.Source = null; return; }
            try
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(fullPath, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                EnemyImage.Source = bmp;
            }
            catch (Exception)
            {
                EnemyImage.Source = null;
            }
        }

        private static string? ResolveImagePath(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string fixedPath = imagePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            string direct = Path.Combine(baseDir, fixedPath);
            if (File.Exists(direct)) return direct;
            string iconsDir = Path.Combine(baseDir, "icons");
            if (!Directory.Exists(iconsDir)) return null;
            string fileName = Path.GetFileName(fixedPath);
            return Directory.GetFiles(iconsDir, fileName, SearchOption.AllDirectories).FirstOrDefault();
        }
    }
}
