namespace CalculatorExtension3.Abstractions
{
    /// <summary>
    /// Allows to use different expressions parsing engines
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Evaluate expression
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Calculated value</returns>
        decimal Evaluate(string expression);
    }
}
