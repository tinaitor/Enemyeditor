using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TimedClicker
{
    // окно только отрисовывает объекты и передаёт события в контроллер
    public partial class MainWindow : Window
    {
        const double SpawnRate = 0.7; // новый объект каждые 0.7 сек
        const double GameTime = 30;   // объекты появляются 30 сек

        DispatcherTimer timer;
        CController? controller;

        public MainWindow()
        {
            InitializeComponent();
            // таймер, обновляющийся каждые 100 миллисекунд
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Size sceneSize = new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            // startTime = SpawnRate, чтобы первый объект появился сразу
            controller = new CController(SpawnRate, SpawnRate, GameTime, sceneSize);
            StartButton.IsEnabled = false;
            timer.Start();
            Draw();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (controller == null) return;

            controller.update(0.1); // обновляем состояние контроллера
            Draw();

            if (controller.isGameOver())
            {
                timer.Stop();
                StartButton.IsEnabled = true;
                MessageBox.Show($"Время вышло! Очки: {controller.getPoints():F0}");
            }
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (controller == null || !timer.IsEnabled) return;

            // позиция курсора относительно сцены
            if (controller.click(e.GetPosition(GameCanvas)))
                Draw();
        }

        // отрисовка объектов контроллера и счётчиков
        private void Draw()
        {
            if (controller == null) return;

            GameCanvas.Children.Clear();
            foreach (CObject obj in controller.getObjects())
                GameCanvas.Children.Add(obj.getSprite());

            PointsText.Text = $"Очки: {controller.getPoints():F0}";
            TimeText.Text = $"Время: {controller.getTimeLeft():F1}";
        }
    }
}
