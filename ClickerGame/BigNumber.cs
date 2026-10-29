using System;
using System.Text;
using System.Linq;

namespace ClickerGame
{
    public class BigNumber
    {
        private int[] number;
        private const int Base = 1000;

        public BigNumber(string numberStr)
        {
            if (string.IsNullOrEmpty(numberStr) || numberStr == "-") numberStr = "0";

            
            numberStr = numberStr.Replace("-", "");

            int len = (numberStr.Length + 2) / 3;
            number = new int[len];
            int pos = numberStr.Length;
            for (int i = 0; i < len; i++)
            {
                int start = Math.Max(0, pos - 3);
                number[i] = int.Parse(numberStr.Substring(start, pos - start));
                pos -= 3;
            }
            TrimLeadingZeros();
        }

        public static BigNumber FromLong(long value) => new BigNumber(value.ToString());

        public override string ToString()
        {
            if (number == null || number.Length == 0) return "0";

            StringBuilder sb = new StringBuilder(number[number.Length - 1].ToString());
            for (int i = number.Length - 2; i >= 0; i--)
                sb.Append(number[i].ToString("D3"));
            return sb.ToString();
        }

        private void TrimLeadingZeros()
        {
            int lastNonZero = number.Length - 1;
            while (lastNonZero > 0 && number[lastNonZero] == 0)
                lastNonZero--;

            if (lastNonZero < number.Length - 1)
            {
                int[] newNum = new int[lastNonZero + 1];
                Array.Copy(number, newNum, newNum.Length);
                number = newNum;
            }
        }

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
            res[len] = carry;

            BigNumber result = new BigNumber("0");
            result.number = res;
            result.TrimLeadingZeros();
            return result;
        }

        
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
            BigNumber result = new BigNumber("0");
            result.number = res;
            result.TrimLeadingZeros();
            return result;
        }
        public BigNumber Multiply(double factor)
        {
            long num = (long)Math.Round(factor * 1000.0);
            if (num == 0) return new BigNumber("0");
            return MultiplyLong(num).DivideLong(1000);
        }
        private BigNumber MultiplyLong(long m)
        {
            int[] res = new int[number.Length + 8];
            long carry = 0;
            for (int i = 0; i < number.Length; i++)
            {
                long t = number[i] * m + carry;
                res[i] = (int)(t % Base);
                carry = t / Base;
            }
            int pos = number.Length;
            while (carry > 0)
            {
                res[pos++] = (int)(carry % Base);
                carry /= Base;
            }
            return FromBlocks(res);
        }
        private BigNumber DivideLong(long d)
        {
            int[] res = new int[number.Length];
            long remainder = 0;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                long t = remainder * Base + number[i];
                res[i] = (int)(t / d);
                remainder = t % d;
            }
            return FromBlocks(res);
        }
        private static BigNumber FromBlocks(int[] blocks)
        {
            BigNumber r = new BigNumber("0");
            r.number = blocks;
            r.TrimLeadingZeros();
            return r;
        }
    }
}