using System;

namespace TimedClicker
{
    public class CPlayer
    {
        private bool canClick = true;
        private double timeBeforeClick;
        private CCountdownTimer countdownTimer;

        public CPlayer(double timeBeforeClick)
        {
            this.timeBeforeClick = timeBeforeClick;
            countdownTimer = new CCountdownTimer(timeBeforeClick);
        }

        public bool CanClick
        {
            get { return canClick; }
        }

        public void startCooldown()
        {
            canClick = false;
            countdownTimer.reset();
        }

        public void update(double delta)
        {
            if (canClick) return;
            countdownTimer.update(delta);
            if (countdownTimer.isReady())
                canClick = true;
        }

        public void increaseSpeed(double seconds)
        {
            timeBeforeClick = Math.Max(0.1, timeBeforeClick - seconds);
            countdownTimer = new CCountdownTimer(timeBeforeClick);
        }
    }
}