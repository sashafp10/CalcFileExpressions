using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace CalculatorExtension3.Implementation
{
    public class ScreenCalcExpressionEvaluator : BaseScreenCalcExpressionEvaluator
    {
        private AutomationElement _input;

        protected override decimal GetResultingValue()
        {
            var resStr = _input.GetText();
            var res = decimal.Parse(resStr);
            return res;
        }

        protected override void DiscoverCalc()
        {
            //TODO: akolyada: test calc discovery

            _calcWindow = AutomationElement.RootElement.FindFirst(
                TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, System.Configuration.ConfigurationManager.AppSettings["CalcName"]));

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
