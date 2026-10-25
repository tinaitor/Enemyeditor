using System;
using System.Text;

namespace ClickerGame
{
    public class BigNumber
    {
        private int[] number;
        private const int Base = 1000;

        public BigNumber(string numberStr)
        {
            if (string.IsNullOrEmpty(numberStr)) numberStr = "0";
            int len = (numberStr.Length + 2) / 3;
            number = new int[len];
            int pos = numberStr.Length;
            for (int i = 0; i < len; i++)
            {
                int start = Math.Max(0, pos - 3);
                number[i] = int.Parse(numberStr.Substring(start, pos - start));
                pos -= 3;
            }
        }

        public static BigNumber FromLong(long value) => new BigNumber(value.ToString());

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(number[number.Length - 1].ToString());
            for (int i = number.Length - 2; i >= 0; i--)
                sb.Append(number[i].ToString("D3"));
            return sb.ToString();
        }

        // Сложение
        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            int len = Math.Max(a.number.Length, b.number.Length);
            int[] res = new int[len + 1];
            int carry = 0;
            for (int i = 0; i < len; i++)
            {
                int sum = carry + (i < a.number.Length ? a.number[i] : 0) + (i < b.number.Length ? b.number[i] : 0);
                res[i] = sum % Base;
                carry = sum / Base;
            }
            if (carry > 0) res[len] = carry;

            // Собираем обратно в строку
            StringBuilder sb = new StringBuilder();
            for (int i = res.Length - 1; i >= 0; i--) sb.Append(res[i]);
            return new BigNumber(sb.ToString().TrimStart('0'));
        }

        // Вычитание 
        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            int len = a.number.Length;
            int[] res = new int[len];
            int borrow = 0;
            for (int i = 0; i < len; i++)
            {
                int sub = borrow + (i < b.number.Length ? b.number[i] : 0);
                if (a.number[i] < sub)
                {
                    res[i] = a.number[i] + Base - sub;
                    borrow = 1;
                }
                else
                {
                    res[i] = a.number[i] - sub;
                    borrow = 0;
                }
            }
            StringBuilder sb = new StringBuilder();
            for (int i = res.Length - 1; i >= 0; i--) sb.Append(res[i]);
            return new BigNumber(sb.ToString().TrimStart('0'));
        }
    }
}