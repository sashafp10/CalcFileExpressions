using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace CalculatorExtension3.Tests
{
    public class BasicExpressionsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("1+1", 1, 1, "+")]
        [TestCase("25-17", 25, 17, "-")]
        [TestCase("11.2 *11", 11.2, 11, "*")]
        [TestCase("15/ 17.3", 15, 17.3, "/")]
        public void ExpressionParseBasic(string exprString, decimal left, decimal right, string operationSign)
        {
            var expr = new BasicMathExpression(exprString);
            expr.OriginalExpression.Should().Be(exprString, $"expression should be {exprString}");
            expr.Left.Should().Be(left.ToString(CultureInfo.InvariantCulture), $"left should be {left}");
            expr.LeftValue.Should().Be(left, $"left value should be {left}");
            expr.Right.Should().Be(right.ToString(CultureInfo.InvariantCulture), $"right should be {right}");
            expr.RightValue.Should().Be(right, $"right value should be {right}");
            expr.Operation.Sign.Should().Be(operationSign, $"operation should be {operationSign}");
        }

        [TestCase("11")]
        [TestCase("1sd5/17.3")]
        [TestCase("25&17")]
        [TestCase("25|17")]
        public void ExpressionParseBasic_ArgumentException(string exprString)
        {
            Assert.Throws<ArgumentException>(() => new BasicMathExpression(exprString));
        }
    }
}