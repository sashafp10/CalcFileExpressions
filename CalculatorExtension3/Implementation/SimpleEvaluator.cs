using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CalculatorExtension3.Abstractions;
using NUnit.Framework;

namespace CalculatorExtension3.Implementation
{
    public class SimpleEvaluator: IEvaluator
    {
        private readonly Regex _rexRightBracket = new Regex(@"([^\)])*[\)]{1}");
        
        /// <summary>
        /// Regexp for priority 1 operations
        /// </summary>
        private readonly Regex _rexP1 = new Regex(@"([\d.,m]+)[ ]*[*/]{1}[ ]*([\d.,m]+)");

        /// <summary>
        /// Regexp for priority 1 operations
        /// </summary>
        private readonly Regex _rexP2 = new Regex(@"([\d.,m]+)[ ]*[+-]{1}[ ]*([\d.,m]+)");
        private readonly IExpressionEvaluator _expressionEvaluator = null;
        public IProgressReporter _progressReporter = new InfiniteInPlaceProgressReporter();

        private string _expression;
        private bool _isProgressWorks = true;

        public SimpleEvaluator()
        {
            //TODO: remove after DI
            //_expressionEvaluator = new ScreenCalcExpressionEvaluator();
            _expressionEvaluator = new ScreenMetroCalcExpressionEvaluator();
            //_expressionEvaluator = new SilentExpressionEvaluator();
        }
        
        public SimpleEvaluator(IExpressionEvaluator evaluator)
        {
            _expressionEvaluator = evaluator;
        }

        public decimal Evaluate(string expression)
        {
            expression = PrepareExpression(expression);
            _expression = expression.Replace(" ","");
            _progressReporter.Start();
            while (Next())
            {
                _progressReporter.Step(1);
            }

            _progressReporter.Stop();

            var res = EvaluateNoBrackets(_expression);
            Debug.WriteLine($"Final: {res}");

            return res;
        }

        private string PrepareExpression(string expression)
        {
            string expr = expression;
            foreach (Operation operation in SupportedOperations.Operations)
            {
                expr = expr.Replace($"{operation.Sign}-", $"{operation.Sign}m");
            }

            if (expr[0] == '-')
                expr = $"m{expr.Substring(1)}";

            return expr.Replace("(-", "(m");
        }

        private bool Next()
        {
            //open brackets
            Match m = _rexRightBracket.Match(_expression);
            if (m.Success)
            {
                var expr1 = m.Groups[0].ToString();
                expr1 = expr1.Substring(0, expr1.Length - 1);
                var idx = expr1.LastIndexOf("(", StringComparison.CurrentCultureIgnoreCase);
                expr1 = expr1.Substring(idx + 1);
                var res = EvaluateNoBrackets(expr1);
                var e = _expression;
                _expression = $"{_expression.Substring(0, idx)}{res}{_expression.Substring(idx + 2 + expr1.Length)}";
                Debug.Write($"Next: {e}   ->   {_expression}   ->   ");
                _expression = PrepareExpression(_expression);
                Debug.WriteLine(_expression);
                return true;
            }

            return false;
        }

        private decimal EvaluateNoBrackets(string expr)
        {
            bool moreSteps;
            do
            {
                expr = PrepareExpression(expr);
                Debug.Write($"EvaluateNoBrackets: {expr} => ");
                Match m = _rexP1.Match(expr);
                if (m.Success)
                    expr = EvaluateBasicExpressionAndReplace(expr, m);
                else
                {
                    m = _rexP2.Match(expr);
                    if (m.Success)
                        expr = EvaluateBasicExpressionAndReplace(expr, m);
                }

                Debug.WriteLine(expr);

                moreSteps = m.Success;
            } while (moreSteps);

            if (expr[0] == 'm')
                expr = $"-{expr.Substring(1)}";

            var res = decimal.Parse(expr);

            return res;
        }

        private string EvaluateBasicExpressionAndReplace(string expression, Match m)
        {
            
            var simpleExpression = new BasicMathExpression(m.Groups[0].ToString());
            return $"{expression.Substring(0, m.Groups[0].Index)}{_expressionEvaluator.Evaluate(simpleExpression)}{expression.Substring(m.Groups[0].Index + m.Groups[0].Length)}";
        }
    }
}
