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
            EnemyTypeComboBox.SelectedIndex = 0;
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

        // Показываем поле особого свойства только у нужного типа
        private void EnemyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArmorPanel == null || HealChancePanel == null) return;

            ArmorPanel.Visibility = EnemyTypeComboBox.SelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            HealChancePanel.Visibility = EnemyTypeComboBox.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        // Создаём объект нужного наследника в зависимости от выбранного типа
        private CEnemyTemplate CreateEnemyOfSelectedType()
        {
            switch (EnemyTypeComboBox.SelectedIndex)
            {
                case 1:
                    return new CArmoredEnemyTemplate
                    {
                        Armor = double.TryParse(ArmorTextBox.Text, out double a) ? a : 0
                    };
                case 2:
                    return new CHealingEnemyTemplate
                    {
                        HealChance = double.TryParse(HealChanceTextBox.Text, out double hc) ? hc : 0
                    };
                default:
                    return new CNormalEnemyTemplate();
            }
        }

        private void AddEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (IconsListBox.SelectedIndex < 0) return;

            EnemyIcon icon = iconList.GetIcons()[IconsListBox.SelectedIndex];

            // Проверка значений происходит в сеттерах свойств CEnemyTemplate
            CEnemyTemplate enemy = CreateEnemyOfSelectedType();
            enemy.Name = NameTextBox.Text;
            enemy.Icon = icon;
            enemy.Health = int.TryParse(HealthTextBox.Text, out int h) ? h : 0;
            enemy.GoldReward = long.TryParse(GoldRewardTextBox.Text, out long g) ? g : 0;
            enemy.SpawnChance = double.TryParse(SpawnChanceTextBox.Text, out double sc) ? sc : 0;
            enemy.HealthModifier = double.TryParse(HealthModifierTextBox.Text, out double hm) ? hm : 0;
            enemy.GoldModifier = double.TryParse(GoldModifierTextBox.Text, out double gm) ? gm : 0;

            enemyList.Add(enemy);
            RefreshEnemies();
        }

        private void RemoveEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (EnemiesListBox.SelectedItem is CEnemyTemplate enemy)
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
            if (EnemiesListBox.SelectedItem is CEnemyTemplate enemy)
            {
                SelectedNameText.Text = enemy.Name;
                SelectedTypeText.Text = $"Тип: {enemy.TypeDisplayName}";
                SelectedHealthText.Text = $"Здоровье: {enemy.Health}";
                SelectedGoldRewardText.Text = $"Золото: {enemy.GoldReward}";
                SelectedSpawnChanceText.Text = $"Шанс (вес): {enemy.SpawnChance}";
                SelectedHealthModifierText.Text = $"×HP за уровень: {enemy.HealthModifier}";
                SelectedGoldModifierText.Text = $"×Золото за уровень: {enemy.GoldModifier}";
                SelectedSpecialText.Text = enemy.SpecialDescription;

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
            if (EnemiesListBox.SelectedItem is CEnemyTemplate enemy &&
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
            SelectedTypeText.Text = "";
            SelectedHealthText.Text = "";
            SelectedGoldRewardText.Text = "";
            SelectedSpawnChanceText.Text = "";
            SelectedHealthModifierText.Text = "";
            SelectedGoldModifierText.Text = "";
            SelectedSpecialText.Text = "";
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
