using System;
using System.Text.RegularExpressions;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class SimpleEvaluator: IEvaluator
    {
        private readonly Regex _rexRightBracket = new Regex(@"([^\)])*[\)]{1}");
        private readonly Regex _rexP1 = new Regex(@"([m\d)]+)[/*]([m\d]+)");
        private readonly Regex _rexP2 = new Regex(@"([m\d)]+)[+-]([m\d]+)");
        //private IExpressionEvaluator _expressionEvaluator = new SilentExpressionEvaluator();
        private readonly IExpressionEvaluator _expressionEvaluator = new ScreenCalcExpressionEvaluator();

        private string _expression;
        public decimal Evaluate(string expression)
        {
            expression = PrepareExpression(expression);
            _expression = expression.Replace(" ","");
            int step = 0;
            var cl = Console.CursorLeft;
            while (Next())
            {
                if (++step > 1)
                    Console.CursorLeft -= 1;

                switch (step % 4)
                {
                    case 0:
                        Console.Write("/");
                        break;
                    case 1:
                        Console.Write("-");
                        break;
                    case 2:
                        Console.Write("\\");
                        break;
                    case 3:
                        Console.Write("|");
                        break;
                }
                
            }

            if (cl < Console.CursorLeft)
                Console.CursorLeft -= 1;

            var res = EvaluateNoBrackets(_expression);
            //Console.WriteLine($"Final: {res}");

            return res;
        }

        private string PrepareExpression(string expression)
        {
            string expr = expression;
            foreach (Operation operation in SupportedOperations.Operations)
            {
                expr = expr.Replace($"{operation.Sign}-", $"{operation.Sign}m");
            }

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
                //Console.Write($"Next: {e}   ->   {_expression}   ->   ");
                _expression = PrepareExpression(_expression);
                //Console.WriteLine(_expression);
                return true;
            }

            return false;
        }

        private decimal EvaluateNoBrackets(string expr)
        {
            bool moreSteps;
            do
            {
                //Console.Write($"EvaluateNoBrackets: {expr} => ");
                Match m = _rexP1.Match(expr);
                if (m.Success)
                    expr = EvaluateBasicExpressionAndReplace(expr, m);
                else
                {
                    m = _rexP2.Match(expr);
                    if (m.Success)
                        expr = EvaluateBasicExpressionAndReplace(expr, m);
                }

                //Console.Write($"{expr} => ");
                expr = PrepareExpression(expr);
                //Console.WriteLine(expr);

                moreSteps = m.Success;
            } while (moreSteps);

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
