using System;

namespace TimedClicker
{
    public class CCountdownTimer
    {
        private double targetTime;
        private double currentTime;

        public CCountdownTimer(double targetTime)
        {
            this.targetTime = targetTime;
            currentTime = 0;
        }

        public bool isReady()
        {
            return currentTime >= targetTime;
        }

        public void update(double delta)
        {
            currentTime += delta;
        }

        public void reset()
        {
            currentTime = 0;
        }

        public double getTimeLeft()
        {
            return Math.Max(0, targetTime - currentTime);
        }

        public void setTargetTime(double newTarget)
        {
            targetTime = newTarget;
        }
    }
}