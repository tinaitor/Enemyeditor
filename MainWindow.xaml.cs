using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Enemyeditor
{
    public partial class MainWindow : Window
    {
        private CIconList iconList = new CIconList();
        private CEnemyList enemyList = new CEnemyList();

        public MainWindow()
        {
            InitializeComponent();
            ClearSelectedStats();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                iconList.LoadIconsFromFolder(dialog.FolderName);
                IconsListBox.Items.Clear();
                foreach (EnemyIcon icon in iconList.GetIcons())
                {
                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(icon.ImagePath, UriKind.Absolute)),
                        Width = 64,
                        Height = 64,
                        Stretch = Stretch.Uniform
                    };
                    IconsListBox.Items.Add(image);
                }
            }
        }

        private void AddEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (IconsListBox.SelectedIndex < 0) return;

            EnemyIcon icon = iconList.GetIcons()[IconsListBox.SelectedIndex];

            EnemyTemplate enemy = new EnemyTemplate
            {
                Name = NameTextBox.Text,
                Icon = icon,
                Health = int.TryParse(HealthTextBox.Text, out int h) ? h : 0,
                Damage = int.TryParse(DamageTextBox.Text, out int d) ? d : 0,
                GoldReward = int.TryParse(SpeedTextBox.Text, out int s) ? s : 0
            };

            enemyList.Add(enemy);
            RefreshEnemies();
        }

        private void RemoveEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem is EnemyTemplate enemy)
            {
                enemyList.Remove(enemy);
                RefreshEnemies();
                ClearSelectedStats();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить список противников",
                Filter = "JSON-файлы (*.json)|*.json",
                DefaultExt = ".json",
                AddExtension = true,
                FileName = "enemies.json"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                enemyList.SaveToJson(dialog.FileName);
                MessageBox.Show("Список противников сохранён.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить файл: {ex.Message}");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Загрузить список противников",
                Filter = "JSON-файлы (*.json)|*.json"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                enemyList.LoadFromJson(dialog.FileName);
                RefreshEnemies();
                ClearSelectedStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить файл: {ex.Message}");
            }
        }

        private void EnemiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem is EnemyTemplate enemy)
            {
                SelectedNameText.Text = enemy.Name;
                SelectedHealthText.Text = $"Здоровье: {enemy.Health}";
                SelectedDamageText.Text = $"Урон: {enemy.Damage}";
                SelectedSpeedText.Text = $"Золото: {enemy.GoldReward}";

                if (!string.IsNullOrEmpty(enemy.Icon.ImagePath))
                {
                    SelectedIconImage.Source = new BitmapImage(
                        new Uri(enemy.Icon.ImagePath, UriKind.Absolute));
                    OpenImageButton.IsEnabled = true;
                }
                else
                {
                    SelectedIconImage.Source = null;
                    OpenImageButton.IsEnabled = false;
                }
            }
            else
            {
                ClearSelectedStats();
            }
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem is EnemyTemplate enemy &&
                !string.IsNullOrEmpty(enemy.Icon.ImagePath))
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = enemy.Icon.ImagePath,
                        UseShellExecute = true
                    });
            }
        }

        private void ClearSelectedStats()
        {
            SelectedNameText.Text = "";
            SelectedHealthText.Text = "";
            SelectedDamageText.Text = "";
            SelectedSpeedText.Text = "";
            SelectedIconImage.Source = null;
            OpenImageButton.IsEnabled = false;
        }

        private void RefreshEnemies()
        {
            EnemiesListBox.ItemsSource = null;
            EnemiesListBox.ItemsSource = enemyList.GetAll();
        }
    }
}