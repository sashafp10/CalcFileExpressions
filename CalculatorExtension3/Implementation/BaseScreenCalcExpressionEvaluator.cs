using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public abstract class BaseScreenCalcExpressionEvaluator : IExpressionEvaluator
    {
        protected readonly Dictionary<string, InvokePattern> _buttons = new Dictionary<string, InvokePattern>();
        protected int _delay = 100;
        protected AutomationElement _calcWindow;

        public BaseScreenCalcExpressionEvaluator()
        {
            DiscoverCalc();
        }

        public decimal Evaluate(BasicMathExpression expression)
        {
            Clear();

            //left
            if (expression.Left.StartsWith("-") || expression.Left.StartsWith("m"))
            {
                EnterNumber(expression.Left.Substring(1));
                Negate();
            } else
                EnterNumber(expression.Left);

            //operation
            EnterOperation(expression.Operation);

            //reght
            if (expression.Right.StartsWith("-") || expression.Right.StartsWith("m"))
            {
                EnterNumber(expression.Right.Substring(1));
                Negate();
            }
            else
                EnterNumber(expression.Right);

            RunEvaluate();

            var res = GetResultingValue();
            return res;
        }

        protected virtual void Clear()
        {
            _buttons["Clear"].Invoke();
            Thread.Sleep(_delay);
        }

        protected virtual void EnterNumber(string value)
        {
            foreach (char c in value)
            {
                _buttons[c.ToString()].Invoke();
                Thread.Sleep(_delay);
            }
        }

        protected virtual void EnterOperation(Operation operation)
        {
            _buttons[operation.Sign].Invoke();
            Thread.Sleep(_delay);
        }
        protected virtual void RunEvaluate()
        {
            _buttons["="].Invoke();
            Thread.Sleep(_delay);
        }

        protected virtual void Negate()
        {
            _buttons["m"].Invoke();
            Thread.Sleep(_delay);
        }

        protected abstract decimal GetResultingValue();

        protected abstract void DiscoverCalc();
    }
}
