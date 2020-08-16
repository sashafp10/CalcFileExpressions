using System;
using CalculatorExtension3.Implementation;
using FluentAssertions;
using NUnit.Framework;

namespace CalculatorExtension3.Tests
{
    public class SimpleEvaluatorTest
    {
        private readonly SimpleEvaluator _se = new SimpleEvaluator(new SilentExpressionEvaluator());

        [SetUp]
        public void Setup()
        {
            var se = new SimpleEvaluator(new SilentExpressionEvaluator());
        }

        [TestCase("1+1", 2)]
        [TestCase("25-17", 8)]
        [TestCase("11.2 *11", 123.2)]
        [TestCase("15/ 17.3", 0.8670520231213873)]
        [TestCase("0 *0", 0)]
        [TestCase("0 /700", 0)]
        [TestCase("5-5 /100", 4.95)]
        [TestCase("(5-5) /100", 0)]
        [TestCase("(5-50) /5", -9)]
        [TestCase("34+22-3332+1", -3275)]
        public void EvaluateTests(string exprString, decimal expected)
        {
            var res = _se.Evaluate(exprString);
            decimal.Round(res, 5).Should().Be(decimal.Round(expected, 5));
        }

        [TestCase("0 /0", 0)]
        public void EvaluateDivizionByZeroTests(string exprString, decimal expected)
        {
            Assert.Throws<DivideByZeroException>(() => _se.Evaluate(exprString));
        }
    }
}
