using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TimedClicker
{
    public partial class MainWindow : Window
    {
        const double SpawnRate = 0.7;
        const double GameTime = 30;
        const double Cooldown = 0.3;

        DispatcherTimer timer;
        CController controller;
        bool started = false;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;

            controller = new CController(SpawnRate, GameTime, Cooldown);
            // подписка на события контроллера
            controller.addObject += Controller_AddObject;
            controller.removeObject += Controller_RemoveObject;
            controller.removeObject += logMessage;
        }

        // обработчик: контроллер создал объект — добавляем его спрайт на сцену
        private void Controller_AddObject(object sender, CControllerEventArgs e)
        {
            GameCanvas.Children.Add(e.Object.getSprite());
        }

        // обработчик: объект пропал (кликнули или истекло время) — убираем спрайт со сцены
        private void Controller_RemoveObject(object sender, CControllerEventArgs e)
        {
            GameCanvas.Children.Remove(e.Object.getSprite());
        }

        public void logMessage(object sender, CControllerEventArgs e)
        {
            if (e.msg != "")
                message_log.Items.Insert(0, e.msg);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Size sceneSize = new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            message_log.Items.Clear();
            controller.start(sceneSize);
            started = true;
            StartButton.IsEnabled = false;
            timer.Start();
            Draw();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!started) return;

            controller.update(0.1);
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
            if (!started || !timer.IsEnabled) return;

            controller.click(e.GetPosition(GameCanvas));
            Draw();
        }

        private void Draw()
        {
            // объекты на сцену добавляются/убираются через события,
            // здесь обновляется только текст
            PointsText.Text = $"Очки: {controller.getPoints():F0}";
            TimeText.Text = $"Время: {controller.getTimeLeft():F1}";

            double cd = controller.getCooldownLeft();
            if (cd > 0)
                CooldownText.Text = $"Перезарядка: {cd:F2} с";
            else
                CooldownText.Text = "Перезарядка: готов";
        }
    }
}