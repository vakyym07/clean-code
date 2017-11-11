using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using ControlDigit;
using NUnit.Framework;

namespace ControlDigit
{
    public static class ControlDigitExtensions
    {
        public static int ControlDigit(this long number)
        {
            int sum = 0;
            int factor = 1;
            do
            {
                int digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;

            }
            while (number > 0);

            int result = sum % 11;
            if (result == 10)
                result = 1;
            return result;
        }

        public static int ControlDigit2(this long number)
        {
            return number.ControlDigit3(() => new[] {1, 3});
        }

        private static int ControlDigit3(this long number, Func<int[]> getWeights)
        {
            var result = number
                       .GetAllDigits()
                       .WeightedSum(getWeights()) % 11;
            return result == 10 ? 1 : result;
        }

        public static int WeightedSum(this IEnumerable<int> source, IEnumerable<int> weights) 
            => source.Zip(weights.Cycle(), (a, b) => a * b).Sum();


        public static IEnumerable<int> GetAllDigits(this long number)
        {
            do
            {
                int digit = (int) (number % 10);
                yield return digit;
                number /= 10;
            } while (number > 0);
        }

        public static int Modulo(this int number, int divisor)
        {
            var result = number % divisor;
            if (result == 10)
                return 1;

            return result;
        }

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            while (true)
            {
                foreach (var item in list)
                {
                    yield return item;
                }
            }
        }
    }

    [TestFixture]
    public class ControlDigitExtensions_Tests
    {
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(9, ExpectedResult = 9)]
        [TestCase(10, ExpectedResult = 3)]
        [TestCase(15, ExpectedResult = 8)]
        [TestCase(17, ExpectedResult = 1)]
        [TestCase(18, ExpectedResult = 0)]
        public int TestControlDigit(long x)
        {
            return x.ControlDigit();
        }

        [Test]
        public void CompareImplementations()
        {
            for (long i = 0; i < 100000; i++)
                Assert.AreEqual(i.ControlDigit(), i.ControlDigit2());
        }
    }

    [TestFixture]
    public class ControlDigit_PerformanceTests
    {
        [Test]
        public void TestControlDigitSpeed()
        {
            var count = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                12345678L.ControlDigit();
            Console.WriteLine("Old " + sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < count; i++)
                12345678L.ControlDigit2();
            Console.WriteLine("New " + sw.Elapsed);
        }
    }
}