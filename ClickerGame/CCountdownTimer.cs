using System;

namespace ClickerGame
{
    // Adapted from TimedClicker: the player owns the timer, not the window.
    public sealed class CCountdownTimer
    {
        private double elapsed;
        public double Duration { get; private set; }
        public double Remaining => Math.Max(0, Duration - elapsed);
        public bool IsReady => Remaining <= 0;

        public CCountdownTimer(double duration)
        {
            Duration = duration;
            elapsed = duration;
        }

        public void Start() => elapsed = 0;
        public void Update(double seconds) => elapsed += Math.Max(0, seconds);
        public void SetDuration(double duration) => Duration = Math.Max(0.1, duration);
    }
}
