using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class ScreenCalcExpressionEvaluator : IExpressionEvaluator
    {
        private AutomationElement _calcWindow;
        private readonly Dictionary<string, InvokePattern> _buttons = new Dictionary<string, InvokePattern>();
        private AutomationElement _input;
        private const int _delay = 300;

        public ScreenCalcExpressionEvaluator()
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
                _buttons["m"].Invoke();
                Thread.Sleep(_delay);
            } else
                EnterNumber(expression.Left);

            //operation
            _buttons[expression.Operation.Sign].Invoke();
            Thread.Sleep(_delay);

            //reght
            if (expression.Right.StartsWith("-") || expression.Right.StartsWith("m"))
            {
                EnterNumber(expression.Right.Substring(1));
                _buttons["m"].Invoke();
                Thread.Sleep(_delay);
            }
            else
                EnterNumber(expression.Right);

            _buttons["="].Invoke();
            Thread.Sleep(_delay);

            var resStr = _input.GetText();
            var res = decimal.Parse(resStr);
            return res;
        }

        public void Clear()
        {
            _buttons["Clear"].Invoke();
            Thread.Sleep(_delay);
        }

        private void EnterNumber(string value)
        {
            foreach (char c in value)
            {
                _buttons[c.ToString()].Invoke();
                Thread.Sleep(_delay);
            }
        }

        private void DiscoverCalc()
        {
            //TODO: akolyada: test calc discovery

            _calcWindow = AutomationElement.RootElement.FindFirst(
                TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, "Calculator"));

            if (_calcWindow == null)
                throw new NullReferenceException("Calculator is not opened. Please run windows calculator and run again.");

            #region init buttons

            Dictionary<string, string> keys = new Dictionary<string, string>()
            {
                {"+", "Add"},
                {"-", "Subtract"},
                {"*", "Multiply"},
                {"/", "Divide"},
                {".", "Decimal separator"},
                {"=", "Equals"},
                {"m", "Negate"},
                {"Clear", "Clear"}
            };


            for (int i = 0; i < 10; i++)
            {
                keys.Add(i.ToString(), i.ToString());
            }

            foreach (var key in keys)
            {
                var btns = _calcWindow.FindAll(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, key.Value));

                foreach (AutomationElement btn in btns)
                {
                    object invokePattern;
                    if (btn.TryGetCurrentPattern(InvokePattern.Pattern, out invokePattern))
                    {
                        _buttons.Add(key.Key, invokePattern as InvokePattern);
                        break;
                    }
                }

            }

            #endregion

            #region init input

            var items = _calcWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, "Static"));
            foreach (AutomationElement item in items)
            {
                string txt = item.GetText();
                if (txt == "0")
                {
                    _input = item;
                    break;
                }
            }

            #endregion
        }
    }
}
