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
                Speed = int.TryParse(SpeedTextBox.Text, out int s) ? s : 0
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
            }
        }

        private void RefreshEnemies()
        {
            EnemiesListBox.ItemsSource = null;
            EnemiesListBox.ItemsSource = enemyList.GetAll();
        }
    }
}