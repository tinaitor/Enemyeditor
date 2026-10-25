using ClickerGame;
using System.Windows;

namespace WpfApp48
{
    public partial class MainWindow : Window
    {
        private readonly EnemyRepository repository = new EnemyRepository();

        public MainWindow()
        {
            InitializeComponent();
            repository.LoadFromJson("enemies.json");

            MessageBox.Show(
                "Загружено противников: " + repository.GetTemplates().Count);
        }
    }
}
