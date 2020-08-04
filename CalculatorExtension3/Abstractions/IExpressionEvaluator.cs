namespace CalculatorExtension3.Abstractions
{
    /// <summary>
    /// Evaluate a value of a simple parsed expression. It allows to use different evaluation engines
    /// </summary>
    public interface IExpressionEvaluator
    {
        decimal Evaluate(BasicMathExpression expression);
    }
}
