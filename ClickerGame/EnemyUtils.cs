namespace ClickerGame
{
    public static class EnemyUtils
    {
        public static BigNumber Scale(BigNumber baseValue, double modifier, int level)
        {
            var result = baseValue;
            for (int i = 0; i < level; i++)
            {
                var next = result.Multiply(modifier);
                if (modifier > 1.0 && next.ToString() == result.ToString())
                    next = result + BigNumber.FromLong(1);
                result = next;
            }
            return result;
        }
    }
}
