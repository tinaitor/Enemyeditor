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
        CController? controller;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Size sceneSize = new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            controller = new CController(SpawnRate, SpawnRate, GameTime, Cooldown, sceneSize);
            StartButton.IsEnabled = false;
            timer.Start();
            Draw();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (controller == null) return;

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
            if (controller == null || !timer.IsEnabled) return;

            controller.click(e.GetPosition(GameCanvas));
            Draw();
        }

        private void Draw()
        {
            if (controller == null) return;

            GameCanvas.Children.Clear();
            foreach (CCollectable obj in controller.getObjects())
                GameCanvas.Children.Add(obj.getSprite());

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