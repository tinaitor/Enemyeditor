using ClickerGame;
using System;
using System.ComponentModel;
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
        private readonly PlayerSaver playerSaver = new PlayerSaver();
        private readonly CController bonuses = new CController();
        private readonly DispatcherTimer timer =
            new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly CGameController gameController;

        private Player player = new Player();
        private TimeSpan previousTick;

        private string PlayerSavePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "player.json");

        public MainWindow()
        {
            InitializeComponent();

            repository.LoadFromJson(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "enemies.json"));

            try { player = playerSaver.Load(PlayerSavePath); }
            catch { player = new Player(); }

            gameController = new CGameController(repository, player);
            gameController.EnemySpawned += OnEnemySpawned;
            gameController.EnemyDefeated += OnEnemyDefeated;
            gameController.EnemyChanged += OnEnemyChanged;
            gameController.EnemyDamaged += OnEnemyDamagedInUI;
            gameController.Start();

            bonuses.SphereAdded += OnSphereAdded;
            bonuses.SphereRemoved += OnSphereRemoved;

            timer.Tick += Timer_Tick;
            timer.Start();
            UpdateUI();
        }
        private void OnEnemyDamagedInUI(object? sender, EnemyEventArgs e)
        {
            
            HealthText.Text = "Здоровье: " + e.CurrentHealth;
        }

        private void OnSphereAdded(object? sender, SphereEventArgs e)
        {
            
            BonusCanvas.Children.Add(e.SphereUI);
        }

        private void OnSphereRemoved(object? sender, SphereEventArgs e)
        {
            
            BonusCanvas.Children.Remove(e.SphereUI);
        }
        private void OnEnemySpawned(object? sender, CEnemyEventArgs e)
        {
            UpdateEnemyDisplay(e.Enemy);
        }

        private void OnEnemyChanged(object? sender, CEnemyEventArgs e)
        {
            UpdateEnemyDisplay(e.Enemy);
        }

        private void OnEnemyDefeated(object? sender, CEnemyEventArgs e)
        {
            Debug.WriteLine($"Побеждён: {e.Enemy.Name}");
        }

        private void UpdateEnemyDisplay(IEnemy? enemy)
        {
            if (enemy == null)
            {
                EnemyNameText.Text = "Врагов нет";
                HealthText.Text = "HP: -";
                EnemyImage.Source = null;
                return;
            }

            EnemyNameText.Text = enemy.Name;
            HealthText.Text = "Здоровье: " + enemy.Health;

            string? fullPath = ResolveImagePath(enemy.ImagePath);
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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeSpan now = clock.Elapsed;
            double delta = Math.Min(0.25, (now - previousTick).TotalSeconds);
            previousTick = now;

            player.Update(delta);
            if (gameController.CurrentEnemy != null)
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

            gameController.Attack();
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

        private void UpgradeGoldSphereButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.TryUpgradeGoldSphere()) UpdateUI();
        }

        private void UpgradeCooldownSphereButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.TryUpgradeCooldownSphere()) UpdateUI();
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            try { playerSaver.Save(player, PlayerSavePath); }
            catch { }
        }

        private void UpdateUI()
        {
            GoldText.Text = "Золото: " + player.Gold;
            DamageText.Text = "Урон: " + player.AttackDamage;

            CooldownText.Text = player.CanAttack
                ? $"Атака готова (кулдаун {player.Cooldown:F2} с)"
                : $"Перезарядка: {player.CooldownLeft:F2} с";

            BoostText.Text = player.DamageBoostLeft > 0
                ? $"Усиление урона: x2 ({player.DamageBoostLeft:F1} с)"
                : "Усиление урона: нет";

            CooldownBoostText.Text = player.CooldownBoostLeft > 0
                ? $"Усиление скорости: x2 ({player.CooldownBoostLeft:F1} с)"
                : "Усиление скорости: нет";

            DoubleStrikeText.Text = player.DoubleStrikeChance > 0
                ? $"Шанс двойного клика: {player.DoubleStrikeChance * 100:F1}%"
                : "Шанс двойного клика: 0%";

            UpgradeDamageButton.Content =
                $"Улучшить урон ({player.UpgradeCost} золота)";
            UpgradeDamageButton.IsEnabled =
                player.Gold.CompareTo(player.UpgradeCost) >= 0;

            UpgradeCooldownButton.Content =
                $"Ускорить атаку (сейчас {player.BaseCooldown:F2} с, {player.CooldownUpgradeCost} золота)";
            UpgradeCooldownButton.IsEnabled =
                player.CanUpgradeCooldown &&
                player.Gold.CompareTo(player.CooldownUpgradeCost) >= 0;

            UpgradeGoldSphereButton.Content =
                $"Улучшить золотую сферу, ур. {player.GoldBonusLevel} " +
                $"({player.GoldBonusUpgradeCost} золота)";
            UpgradeGoldSphereButton.IsEnabled =
                player.Gold.CompareTo(player.GoldBonusUpgradeCost) >= 0 &&
                player.GoldBonusLevel < 20;

            UpgradeCooldownSphereButton.Content =
                $"Улучшить шанс двойного клика, ур. {player.CooldownSphereLevel} " +
                $"({player.CooldownSphereUpgradeCost} золота)";
            UpgradeCooldownSphereButton.IsEnabled =
                player.CanUpgradeCooldownSphere &&
                player.Gold.CompareTo(player.CooldownSphereUpgradeCost) >= 0;

            var enemy = gameController.CurrentEnemy;
            if (enemy == null)
            {
                EnemyNameText.Text = "Врагов нет";
                HealthText.Text = "HP: -";
                return;
            }
        }

        private static string? ResolveImagePath(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string fixedPath = imagePath
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            string direct = Path.Combine(baseDir, fixedPath);
            if (File.Exists(direct)) return direct;

            string iconsDir = Path.Combine(baseDir, "icons");
            if (!Directory.Exists(iconsDir)) return null;

            string fileName = Path.GetFileName(fixedPath);
            return Directory.GetFiles(iconsDir, fileName, SearchOption.AllDirectories).FirstOrDefault();
        }
    }
}