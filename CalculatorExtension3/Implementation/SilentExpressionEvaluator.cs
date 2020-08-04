using System;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class SilentExpressionEvaluator: IExpressionEvaluator
    {
        public decimal Evaluate(BasicMathExpression expression)
        {
            if(expression.Operation.Equals(SupportedOperations.Plus))
                return expression.LeftValue + expression.RightValue;
            
            if(expression.Operation.Equals(SupportedOperations.Minus))
                return expression.LeftValue - expression.RightValue;
            
            if(expression.Operation.Equals(SupportedOperations.Multiply))
                return expression.LeftValue * expression.RightValue;
            
            if(expression.Operation.Equals(SupportedOperations.Divide))
                return expression.LeftValue / expression.RightValue;

            throw  new NotSupportedException($"The operation '{expression.Operation.Sign}' is not supported");
        }
    }
}
