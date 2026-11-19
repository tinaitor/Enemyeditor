using System;

namespace ClickerGame
{
    public sealed class CCountdownTimer
    {
        private double elapsed;
        public double Duration { get; private set; }
        public double Remaining => Math.Max(0, Duration - elapsed);
        public bool IsReady => Remaining <= 0;

        public CCountdownTimer(double duration)
        {
            Duration = Math.Max(0.01, duration);
            elapsed = Duration;
        }

        public void Start() => elapsed = 0;
        public void Update(double seconds) => elapsed += Math.Max(0, seconds);
        public void SetDuration(double duration)
        {
            duration = Math.Max(0.01, duration);
            double progress = Duration > 0 ? Math.Clamp(elapsed / Duration, 0, 1) : 1;
            Duration = duration;
            elapsed = progress * duration;
        }
    }
}