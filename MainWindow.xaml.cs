using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enemyeditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CIconList iconList = new CIconList();
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
    }
}