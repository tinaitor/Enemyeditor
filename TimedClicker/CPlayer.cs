using System;

namespace TimedClicker
{
    public class CPlayer
    {
        private double timeBeforeClick;
        private CCountdownTimer countdownTimer;

        public CPlayer(double timeBeforeClick)
        {
            this.timeBeforeClick = timeBeforeClick;
            countdownTimer = new CCountdownTimer(timeBeforeClick);
        }

        public bool CanClick
        {
            get { return countdownTimer.isReady(); }
        }

        public void startCooldown()
        {
            countdownTimer.reset();
        }

        public void update(double delta)
        {
            countdownTimer.update(delta);
        }

        public void increaseSpeed(double speedModifier)
        {
            timeBeforeClick = Math.Max(0.1, timeBeforeClick - speedModifier);
            countdownTimer = new CCountdownTimer(timeBeforeClick);
        }
    }
}