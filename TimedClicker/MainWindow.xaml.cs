using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TimedClicker
{
    // Окно только рисует и передаёт события в контроллер — логики игры здесь нет.
    public partial class MainWindow : Window
    {
        const double TickSeconds = 0.1;   // шаг таймера
        const double SpawnRate = 0.7;     // новый кружок каждые 0.7 сек
        const double GameDuration = 30;   // кружки появляются 30 сек

        DispatcherTimer timer;
        CController? controller;

        public MainWindow()
        {
            InitializeComponent();

            // таймер, срабатывающий каждые 100 миллисекунд
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(TickSeconds);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Size sceneSize = new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            controller = new CController(SpawnRate, GameDuration, sceneSize);

            StartButton.IsEnabled = false;
            timer.Start();
            Redraw();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (controller == null) return;

            controller.update(TickSeconds); // обновляем состояние контроллера
            Redraw();

            if (controller.isFinished())
            {
                timer.Stop();
                StartButton.IsEnabled = true;
                MessageBox.Show($"Время вышло! Очки: {controller.getPoints():F0}");
            }
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (controller == null || !timer.IsEnabled) return;

            // координаты курсора относительно зоны игры
            if (controller.click(e.GetPosition(GameCanvas)))
                Redraw();
        }

        // перерисовка сцены по текущему состоянию контроллера
        private void Redraw()
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
