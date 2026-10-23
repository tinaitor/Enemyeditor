using ClickerGame;
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

namespace WpfApp48
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EnemyRepository repository = new EnemyRepository();
        public MainWindow()
        {
            InitializeComponent();
            repository.LoadFromJson("enemies.json");
            MessageBox.Show("Загружено противников: " + repository.GetTemplates().Count);
        }
    }
}