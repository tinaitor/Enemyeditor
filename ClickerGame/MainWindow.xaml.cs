using ClickerGame; // Твои классы
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp48 // Окно остается в этом namespace
{
    public partial class MainWindow : Window
    {
        private readonly EnemyRepository repository = new EnemyRepository();
        private readonly Player player = new Player(BigNumber.FromLong(1)); // Игрок с уроном 1
        private Enemy? currentEnemy; // Текущий враг на экране

        public MainWindow()
        {
            InitializeComponent();
            repository.LoadFromJson("enemies.json");

            // Берем первого врага из списка
            var templates = repository.GetTemplates();
            if (templates != null && templates.Count > 0)
                currentEnemy = new Enemy(templates[0]);

            UpdateUI(); // Обновляем экран
        }

        // ==========================================================
        // ТВОЯ ЧАСТЬ: Обработчик клика (урон + начисление золота)
        // ==========================================================
        private void EnemyButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null) return;

            // 1. Игрок бьет врага. Метод вернет true, если враг умер.
            bool isDefeated = currentEnemy.TakeDamage(player.Damage);

            // 2. Если враг убит
            if (isDefeated)
            {
                // Начисляем золото через метод (инкапсуляция в действии!)
                player.AddGold(currentEnemy.Reward);

                // ВЫБОР СЛЕДУЮЩЕГО ВРАГА (Заглушка для напарника)
                // В идеале тут должен быть вызов метода из EnemyRepository с учетом шансов
                var templates = repository.GetTemplates();
                if (templates != null && templates.Count > 0)
                    currentEnemy = new Enemy(templates[0]); // Пока спавним первого
            }

            // 3. Обновляем экран после каждого клика
            UpdateUI();
        }

        // Метод для обновления текста и картинок на экране
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

            // Загружаем картинку
            if (!string.IsNullOrEmpty(currentEnemy.ImagePath) && File.Exists(currentEnemy.ImagePath))
                EnemyImage.Source = new BitmapImage(new Uri(Path.GetFullPath(currentEnemy.ImagePath)));
        }
    }
}