using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class ScreenMetroCalcExpressionEvaluator : IExpressionEvaluator
    {
        private AutomationElement _calcWindow;
        private readonly Dictionary<string, InvokePattern> _buttons = new Dictionary<string, InvokePattern>();
        private AutomationElement _input;
        private const int _delay = 300;
        private readonly Regex _numbersRex = new Regex(@"([\d]+)");

        //[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //// Activate an application window.
        //[DllImport("USER32.DLL")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);

        public ScreenMetroCalcExpressionEvaluator()
        {
            // Get a handle to the Calculator application. The window class 
            // and window name were obtained using the Spy++ tool.

            //Here is one more method for working with a calc. It's more common but I am not sure which one is preferable
            //IntPtr calculatorHandle = FindWindow("ApplicationFrameWindow", "Калькулятор");

            // Verify that Calculator is a running process. 
            //if (calculatorHandle == IntPtr.Zero)
            //{
            //    Console.WriteLine("Calculator is not running.");
            //    return;
            //}

            //// Make Calculator the foreground application and send it  
            //// a set of calculations.
            //SetForegroundWindow(calculatorHandle);
            //SendKeys.SendWait("111");
            //SendKeys.SendWait("*");
            //SendKeys.SendWait("11");
            //SendKeys.SendWait("=");
            //SendKeys.SendWait("Ctrl+C");
            //Console.WriteLine($"Clipboard.GetText(): {Clipboard.GetText()}");
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

            var resStr = GetResultText();
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
                new PropertyCondition(AutomationElement.NameProperty, System.Configuration.ConfigurationManager.AppSettings["CalcName"]));

            if (_calcWindow == null)
            {
                throw new NullReferenceException("Calculator is not opened. Please run windows calculator and run again.");
            }

            //DiscoverNames();

            #region init buttons

            Dictionary<string, string> keys = new Dictionary<string, string>()
            {
                {"+", "plusButton"},
                {"-", "minusButton"},
                {"*", "multiplyButton"},
                {"/", "divideButton"},
                {".", "decimalSeparatorButton"},
                {"=", "equalButton"},
                {"m", "negateButton"},
                {"Clear", "clearButton"}
            };


            for (int i = 0; i < 10; i++)
            {
                keys.Add(i.ToString(), $"num{i}Button");
            }

            foreach (var key in keys)
            {
                var btns = _calcWindow.FindAll(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, key.Value));



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

            DiscoverInput();

            #endregion
        }

        private void DiscoverInput()
        {
            _input = _calcWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "CalculatorResults"))[0];
        }

        private string GetResultText()
        {
            if (_input == null)
                DiscoverInput();

            var st = _input.GetText();

            var symbols = st.ToCharArray().Where(n => char.IsDigit(n) || n == '-').Select(n => n).ToArray();
            var res = new string(symbols);

            return res;
        }

        private void DiscoverNames()
        {
            InvokePattern b = null;
            var btns = _calcWindow.FindAll(TreeScope.Descendants, Condition.TrueCondition);
            foreach (AutomationElement btn in btns)
            {
                Console.WriteLine($"{btn.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty)} - {btn.GetCurrentPropertyValue(AutomationElement.NameProperty)} - {btn.Current.Name} ({btn.Current.ClassName})");
                if (btn.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty) == "num5Button")
                {
                    object invokePattern;
                    if (btn.TryGetCurrentPattern(InvokePattern.Pattern, out invokePattern))
                    {
                        b = invokePattern as InvokePattern;
                    }
                }

            }

            if (b != null)
                b.Invoke();
        }
    }
}
