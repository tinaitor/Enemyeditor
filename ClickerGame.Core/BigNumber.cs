using System;
using System.Text;

namespace ClickerGame.Core
{
    /// <summary>Неотрицательное целое число произвольной длины.</summary>
    public sealed class BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
    {
        private const int Base = 1000;
        // Младший блок хранится первым. Массив никогда не выходит наружу.
        private readonly int[] digits;

        public static BigNumber Zero { get; } = new BigNumber(0);
        public static BigNumber One { get; } = new BigNumber(1);
        public bool IsZero => digits.Length == 1 && digits[0] == 0;

        public BigNumber(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Число не может быть отрицательным.");

            if (value == 0)
            {
                digits = new[] { 0 };
                return;
            }

            int[] buffer = new int[4];
            int count = 0;
            while (value > 0)
            {
                buffer[count++] = value % Base;
                value /= Base;
            }
            digits = new int[count];
            Array.Copy(buffer, digits, count);
        }

        public BigNumber(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            value = value.Trim();
            if (value.Length == 0)
                throw new FormatException("Пустая строка не является числом.");

            for (int i = 0; i < value.Length; i++)
                if (value[i] < '0' || value[i] > '9')
                    throw new FormatException("Допустимы только цифры от 0 до 9.");

            int[] blocks = new int[(value.Length + 2) / 3];
            int blockIndex = 0;
            for (int end = value.Length; end > 0; end -= 3)
            {
                int start = Math.Max(0, end - 3);
                int block = 0;
                for (int i = start; i < end; i++)
                    block = block * 10 + (value[i] - '0');
                blocks[blockIndex++] = block;
            }
            digits = Trim(blocks);
        }

        private BigNumber(int[] ownedDigits)
        {
            digits = Trim(ownedDigits);
        }

        private static int[] Trim(int[] blocks)
        {
            int count = blocks.Length;
            while (count > 1 && blocks[count - 1] == 0)
                count--;
            if (count == blocks.Length)
                return blocks;
            int[] result = new int[count];
            Array.Copy(blocks, result, count);
            return result;
        }

        public int CompareTo(BigNumber? other)
        {
            if (other is null) return 1;
            if (digits.Length != other.digits.Length)
                return digits.Length.CompareTo(other.digits.Length);
            for (int i = digits.Length - 1; i >= 0; i--)
                if (digits[i] != other.digits[i])
                    return digits[i].CompareTo(other.digits[i]);
            return 0;
        }

        public bool Equals(BigNumber? other) => other is not null && CompareTo(other) == 0;
        public override bool Equals(object? obj) => obj is BigNumber other && Equals(other);

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            foreach (int digit in digits) hash.Add(digit);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(digits[digits.Length - 1].ToString());
            for (int i = digits.Length - 2; i >= 0; i--)
                result.Append(digits[i].ToString("D3"));
            return result.ToString();
        }

        private BigNumber Add(BigNumber other)
        {
            int count = Math.Max(digits.Length, other.digits.Length);
            int[] result = new int[count + 1];
            int carry = 0;
            for (int i = 0; i < count; i++)
            {
                int a = i < digits.Length ? digits[i] : 0;
                int b = i < other.digits.Length ? other.digits[i] : 0;
                int sum = a + b + carry;
                result[i] = sum % Base;
                carry = sum / Base;
            }
            result[count] = carry;
            return new BigNumber(result);
        }

        private BigNumber Subtract(BigNumber other)
        {
            if (CompareTo(other) < 0)
                throw new ArgumentOutOfRangeException(nameof(other), "Результат не может быть отрицательным.");

            int[] result = new int[digits.Length];
            int borrow = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                int b = i < other.digits.Length ? other.digits[i] : 0;
                int difference = digits[i] - b - borrow;
                borrow = difference < 0 ? 1 : 0;
                result[i] = difference < 0 ? difference + Base : difference;
            }
            return new BigNumber(result);
        }

        private BigNumber Multiply(BigNumber other)
        {
            int[] result = new int[digits.Length + other.digits.Length];
            for (int i = 0; i < digits.Length; i++)
            {
                int carry = 0;
                for (int j = 0; j < other.digits.Length; j++)
                {
                    int product = result[i + j] + digits[i] * other.digits[j] + carry;
                    result[i + j] = product % Base;
                    carry = product / Base;
                }
                result[i + other.digits.Length] += carry;
            }
            return new BigNumber(result);
        }

        private BigNumber MultiplyByBlock(int block)
        {
            int[] result = new int[digits.Length + 1];
            int carry = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                int product = digits[i] * block + carry;
                result[i] = product % Base;
                carry = product / Base;
            }
            result[digits.Length] = carry;
            return new BigNumber(result);
        }

        private BigNumber ShiftAndAppend(int block)
        {
            int[] result = new int[digits.Length + 1];
            result[0] = block;
            Array.Copy(digits, 0, result, 1, digits.Length);
            return new BigNumber(result);
        }

        private BigNumber Divide(BigNumber divisor)
        {
            if (divisor.IsZero) throw new DivideByZeroException();
            if (CompareTo(divisor) < 0) return Zero;

            int[] quotient = new int[digits.Length];
            BigNumber remainder = Zero;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                remainder = remainder.ShiftAndAppend(digits[i]);
                int low = 0;
                int high = Base - 1;
                while (low <= high)
                {
                    int middle = (low + high) / 2;
                    if (divisor.MultiplyByBlock(middle).CompareTo(remainder) <= 0)
                        low = middle + 1;
                    else
                        high = middle - 1;
                }
                quotient[i] = high;
                remainder = remainder.Subtract(divisor.MultiplyByBlock(high));
            }
            return new BigNumber(quotient);
        }

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            return a.Add(b);
        }

        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            return a.Subtract(b);
        }

        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            return a.Multiply(b);
        }

        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            return a.Divide(b);
        }

        public static bool operator ==(BigNumber? a, BigNumber? b) => ReferenceEquals(a, b) || (a is not null && a.Equals(b));
        public static bool operator !=(BigNumber? a, BigNumber? b) => !(a == b);
        public static bool operator <(BigNumber a, BigNumber b) => a.CompareTo(b) < 0;
        public static bool operator >(BigNumber a, BigNumber b) => a.CompareTo(b) > 0;
        public static bool operator <=(BigNumber a, BigNumber b) => a.CompareTo(b) <= 0;
        public static bool operator >=(BigNumber a, BigNumber b) => a.CompareTo(b) >= 0;
    }
}
